using System.Text;
using geoStudio.API.Hubs;
using geoStudio.API.Middleware;
using geoStudio.Application;
using geoStudio.Infrastructure;
using geoStudio.Infrastructure.Persistence;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

// ──────────────────────────────────────────────
// Bootstrap Serilog early so startup errors are captured
// ──────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting geoStudio API...");

    var builder = WebApplication.CreateBuilder(args);

    // ──────────────────────────────────────────────
    // Serilog
    // ──────────────────────────────────────────────
    builder.Host.UseSerilog((ctx, services, config) => config
        .ReadFrom.Configuration(ctx.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File(
            path: "logs/geostudio-.log",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 14));

    // ──────────────────────────────────────────────
    // Application & Infrastructure layers
    // ──────────────────────────────────────────────
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    // ──────────────────────────────────────────────
    // JWT Authentication
    // ──────────────────────────────────────────────
    var jwtKey = builder.Configuration["Jwt:SecretKey"]
        ?? throw new InvalidOperationException("Jwt:SecretKey is missing from configuration.");

    builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30)
            };

            // Support SignalR token from query string
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = ctx =>
                {
                    var accessToken = ctx.Request.Query["access_token"];
                    var path = ctx.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        ctx.Token = accessToken;
                    return Task.CompletedTask;
                }
            };
        });

    builder.Services.AddAuthorization();

    // ──────────────────────────────────────────────
    // CORS
    // ──────────────────────────────────────────────
    var allowedOrigins = builder.Configuration["AllowedOrigins"]
        ?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        ?? ["http://localhost:3000"];

    builder.Services.AddCors(options =>
        options.AddPolicy("FrontendPolicy", policy =>
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials()));

    // ──────────────────────────────────────────────
    // SignalR
    // ──────────────────────────────────────────────
    builder.Services.AddSignalR(options =>
    {
        options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    });

    // ──────────────────────────────────────────────
    // Controllers & API
    // ──────────────────────────────────────────────
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // ──────────────────────────────────────────────
    // Swagger / OpenAPI
    // ──────────────────────────────────────────────
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "geoStudio API",
            Version = "v1",
            Description = "AI Visibility Platform — REST API",
            Contact = new OpenApiContact { Name = "geoStudio Team" }
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Enter: Bearer {token}",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                []
            }
        });

        // Include XML docs
        var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
            options.IncludeXmlComments(xmlPath);
    });

    // ──────────────────────────────────────────────
    // Hangfire
    // ──────────────────────────────────────────────
    var hangfireConnStr = builder.Configuration.GetConnectionString("DefaultConnection")!;
    builder.Services.AddHangfire(config => config
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage(hangfire => hangfire.UseNpgsqlConnection(hangfireConnStr)));

    builder.Services.AddHangfireServer(options =>
    {
        options.WorkerCount = 4;
        options.ServerName = "geoStudio-jobs";
    });

    // ──────────────────────────────────────────────
    // Health checks
    // ──────────────────────────────────────────────
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<GeoStudioDbContext>();

    // ══════════════════════════════════════════════
    // Build the app
    // ══════════════════════════════════════════════
    var app = builder.Build();

    // ──────────────────────────────────────────────
    // Auto-migrate on startup (dev only; use explicit migrations in prod)
    // ──────────────────────────────────────────────
    if (app.Environment.IsDevelopment())
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<GeoStudioDbContext>();
        await db.Database.MigrateAsync();
    }

    // ──────────────────────────────────────────────
    // Middleware pipeline
    // ──────────────────────────────────────────────
    app.UseMiddleware<ErrorHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "geoStudio API v1");
            options.RoutePrefix = string.Empty; // serve at /
        });
    }

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    });

    app.UseHttpsRedirection();
    app.UseCors("FrontendPolicy");
    app.UseAuthentication();
    app.UseAuthorization();

    // Hangfire Dashboard (dev only or behind auth in prod)
    if (app.Environment.IsDevelopment())
    {
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            IsReadOnlyFunc = _ => false
        });
    }

    app.MapControllers();
    app.MapHub<AuditHub>("/hubs/audit");
    app.MapHealthChecks("/health");

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "geoStudio API terminated unexpectedly.");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}

// Needed for integration testing
public partial class Program { }
