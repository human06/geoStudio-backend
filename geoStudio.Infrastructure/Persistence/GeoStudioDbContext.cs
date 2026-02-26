using geoStudio.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace geoStudio.Infrastructure.Persistence;

/// <summary>Entity Framework Core DbContext for geoStudio using PostgreSQL.</summary>
public class GeoStudioDbContext(DbContextOptions<GeoStudioDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<BusinessProfile> BusinessProfiles => Set<BusinessProfile>();
    public DbSet<BusinessLocation> BusinessLocations => Set<BusinessLocation>();
    public DbSet<Competitor> Competitors => Set<Competitor>();
    public DbSet<AIProvider> AIProviders => Set<AIProvider>();
    public DbSet<AIModel> AIModels => Set<AIModel>();
    public DbSet<Persona> Personas => Set<Persona>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Audit> Audits => Set<Audit>();
    public DbSet<AuditQuery> AuditQueries => Set<AuditQuery>();
    public DbSet<AuditPersona> AuditPersonas => Set<AuditPersona>();
    public DbSet<AuditResult> AuditResults => Set<AuditResult>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<ReportSchedule> ReportSchedules => Set<ReportSchedule>();
    public DbSet<geoStudio.Domain.Entities.Task> Tasks => Set<geoStudio.Domain.Entities.Task>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<NotificationPreference> NotificationPreferences => Set<NotificationPreference>();
    public DbSet<AlertRule> AlertRules => Set<AlertRule>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GeoStudioDbContext).Assembly);

        ConfigureUser(modelBuilder);
        ConfigureBusinessProfile(modelBuilder);
        ConfigureBusinessLocation(modelBuilder);
        ConfigureCompetitor(modelBuilder);
        ConfigureAIProvider(modelBuilder);
        ConfigureAIModel(modelBuilder);
        ConfigurePersona(modelBuilder);
        ConfigureTeamMember(modelBuilder);
        ConfigureAudit(modelBuilder);
        ConfigureAuditQuery(modelBuilder);
        ConfigureAuditPersona(modelBuilder);
        ConfigureAuditResult(modelBuilder);
        ConfigureReport(modelBuilder);
        ConfigureReportSchedule(modelBuilder);
        ConfigureTask(modelBuilder);
        ConfigureNotification(modelBuilder);
        ConfigureNotificationPreference(modelBuilder);
        ConfigureAlertRule(modelBuilder);
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(256).IsRequired();
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
            entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
            entity.Property(e => e.AvatarUrl).HasColumnName("avatar_url").HasMaxLength(2048);
            entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            entity.Property(e => e.RefreshToken).HasColumnName("refresh_token");
            entity.Property(e => e.RefreshTokenExpiresAt).HasColumnName("refresh_token_expires_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");

            entity.HasIndex(e => e.Email).IsUnique();
        });
    }

    private static void ConfigureBusinessProfile(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BusinessProfile>(entity =>
        {
            entity.ToTable("business_profiles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.OwnerUserId).HasColumnName("owner_user_id").IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.WebsiteUrl).HasColumnName("website_url").HasMaxLength(2048);
            entity.Property(e => e.IndustryCategory).HasColumnName("industry_category").HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.LogoUrl).HasColumnName("logo_url").HasMaxLength(2048);
            entity.Property(e => e.SubscriptionTier).HasColumnName("subscription_tier").HasConversion<string>();
            entity.Property(e => e.MaxLocations).HasColumnName("max_locations").HasDefaultValue(1);
            entity.Property(e => e.MaxAuditsPerMonth).HasColumnName("max_audits_per_month").HasDefaultValue(50);
            entity.Property(e => e.MaxTeamMembers).HasColumnName("max_team_members").HasDefaultValue(3);
            entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Owner)
                .WithMany(u => u.OwnedBusinesses)
                .HasForeignKey(e => e.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureBusinessLocation(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BusinessLocation>(entity =>
        {
            entity.ToTable("business_locations");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.BusinessId).HasColumnName("business_id").IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Address).HasColumnName("address").HasMaxLength(500);
            entity.Property(e => e.City).HasColumnName("city").HasMaxLength(100);
            entity.Property(e => e.State).HasColumnName("state").HasMaxLength(100);
            entity.Property(e => e.ZipCode).HasColumnName("zip_code").HasMaxLength(20);
            entity.Property(e => e.Country).HasColumnName("country").HasMaxLength(100);
            entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
            entity.Property(e => e.WebsiteUrl).HasColumnName("website_url").HasMaxLength(2048);
            entity.Property(e => e.Latitude).HasColumnName("latitude").HasPrecision(9, 6);
            entity.Property(e => e.Longitude).HasColumnName("longitude").HasPrecision(9, 6);
            entity.Property(e => e.ServiceRadiusKm).HasColumnName("service_radius_km");
            entity.Property(e => e.EnabledForTesting).HasColumnName("enabled_for_testing").HasDefaultValue(true);
            entity.Property(e => e.IsPrimary).HasColumnName("is_primary").HasDefaultValue(false);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Business)
                .WithMany(b => b.Locations)
                .HasForeignKey(e => e.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.BusinessId);
            entity.HasIndex(e => new { e.BusinessId, e.IsPrimary });
        });
    }

    private static void ConfigureCompetitor(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Competitor>(entity =>
        {
            entity.ToTable("competitors");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.BusinessId).HasColumnName("business_id").IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.WebsiteUrl).HasColumnName("website_url").HasMaxLength(2048);
            entity.Property(e => e.IndustryCategory).HasColumnName("industry_category").HasMaxLength(100);
            entity.Property(e => e.LastChecked).HasColumnName("last_checked");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Business)
                .WithMany(b => b.Competitors)
                .HasForeignKey(e => e.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.BusinessId);
        });
    }

    private static void ConfigureTeamMember(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TeamMember>(entity =>
        {
            entity.ToTable("team_members");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.BusinessId).HasColumnName("business_id").IsRequired();
            entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            entity.Property(e => e.Role).HasColumnName("role").HasConversion<string>();
            entity.Property(e => e.InvitedByUserId).HasColumnName("invited_by_user_id");
            entity.Property(e => e.InvitedAt).HasColumnName("invited_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.JoinedAt).HasColumnName("joined_at");
            entity.Property(e => e.Status).HasColumnName("status").HasConversion<string>();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Business)
                .WithMany(b => b.TeamMembers)
                .HasForeignKey(e => e.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(u => u.TeamMemberships)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.InvitedBy)
                .WithMany(u => u.InvitationsSent)
                .HasForeignKey(e => e.InvitedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => new { e.BusinessId, e.UserId }).IsUnique();
        });
    }

    private static void ConfigureAudit(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Audit>(entity =>
        {
            entity.ToTable("audits");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.BusinessId).HasColumnName("business_id").IsRequired();
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(e => e.Status).HasColumnName("status").HasConversion<string>();
            entity.Property(e => e.VisibilityScore).HasColumnName("visibility_score").HasPrecision(5, 2);
            entity.Property(e => e.CitationRate).HasColumnName("citation_rate").HasPrecision(5, 2);
            entity.Property(e => e.AverageRankingPosition).HasColumnName("average_ranking_position");
            entity.Property(e => e.TotalMentions).HasColumnName("total_mentions");
            entity.Property(e => e.TotalTestsCount).HasColumnName("total_tests_count");
            entity.Property(e => e.CompletedTestsCount).HasColumnName("completed_tests_count");
            entity.Property(e => e.StartedAt).HasColumnName("started_at");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.ErrorMessage).HasColumnName("error_message");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Business)
                .WithMany(b => b.Audits)
                .HasForeignKey(e => e.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Location)
                .WithMany(l => l.Audits)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.CreatedBy)
                .WithMany(u => u.CreatedAudits)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.BusinessId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreatedByUserId);
        });
    }

    private static void ConfigureAuditQuery(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditQuery>(entity =>
        {
            entity.ToTable("audit_queries");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.AuditId).HasColumnName("audit_id").IsRequired();
            entity.Property(e => e.QueryText).HasColumnName("query_text").HasMaxLength(500).IsRequired();
            entity.Property(e => e.SearchVolume).HasColumnName("search_volume");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Audit)
                .WithMany(a => a.Queries)
                .HasForeignKey(e => e.AuditId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.AuditId);
        });
    }

    private static void ConfigureAuditPersona(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditPersona>(entity =>
        {
            entity.ToTable("audit_personas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.AuditId).HasColumnName("audit_id").IsRequired();
            entity.Property(e => e.PersonaId).HasColumnName("persona_id").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Audit)
                .WithMany(a => a.Personas)
                .HasForeignKey(e => e.AuditId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Persona)
                .WithMany(p => p.AuditPersonas)
                .HasForeignKey(e => e.PersonaId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.AuditId, e.PersonaId }).IsUnique();
        });
    }

    private static void ConfigureAuditResult(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditResult>(entity =>
        {
            entity.ToTable("audit_results");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.AuditId).HasColumnName("audit_id").IsRequired();
            entity.Property(e => e.QueryId).HasColumnName("query_id").IsRequired();
            entity.Property(e => e.PersonaId).HasColumnName("persona_id").IsRequired();
            entity.Property(e => e.AIModelId).HasColumnName("ai_model_id").IsRequired();
            entity.Property(e => e.IsMentioned).HasColumnName("is_mentioned").HasDefaultValue(false);
            entity.Property(e => e.RankingPosition).HasColumnName("ranking_position");
            entity.Property(e => e.CompetitorMentions).HasColumnName("competitor_mentions").HasDefaultValue(0);
            entity.Property(e => e.CitationSnippet).HasColumnName("citation_snippet");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Audit)
                .WithMany(a => a.Results)
                .HasForeignKey(e => e.AuditId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Query)
                .WithMany(q => q.Results)
                .HasForeignKey(e => e.QueryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Persona)
                .WithMany(p => p.AuditResults)
                .HasForeignKey(e => e.PersonaId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.AIModel)
                .WithMany(m => m.AuditResults)
                .HasForeignKey(e => e.AIModelId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.AuditId);
            entity.HasIndex(e => e.AIModelId);
            entity.HasIndex(e => new { e.AuditId, e.QueryId, e.PersonaId, e.AIModelId }).IsUnique();
        });
    }

    private static void ConfigureReport(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Report>(entity =>
        {
            entity.ToTable("reports");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.AuditId).HasColumnName("audit_id").IsRequired();
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id").IsRequired();
            entity.Property(e => e.ReportType).HasColumnName("report_type").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(500).IsRequired();
            entity.Property(e => e.ContentJson).HasColumnName("content_json");
            entity.Property(e => e.PdfUrl).HasColumnName("pdf_url").HasMaxLength(2048);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Audit)
                .WithMany(a => a.Reports)
                .HasForeignKey(e => e.AuditId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.CreatedBy)
                .WithMany(u => u.CreatedReports)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.AuditId);
            entity.HasIndex(e => e.CreatedByUserId);
        });
    }

    private static void ConfigureReportSchedule(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReportSchedule>(entity =>
        {
            entity.ToTable("report_schedules");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.BusinessId).HasColumnName("business_id").IsRequired();
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id").IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.ReportType).HasColumnName("report_type").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Frequency).HasColumnName("frequency").HasMaxLength(50).IsRequired();
            entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
            entity.Property(e => e.DayOfMonth).HasColumnName("day_of_month");
            entity.Property(e => e.TimeOfDay).HasColumnName("time_of_day");
            entity.Property(e => e.RecipientEmails).HasColumnName("recipient_emails");
            entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            entity.Property(e => e.LastRunAt).HasColumnName("last_run_at");
            entity.Property(e => e.NextRunAt).HasColumnName("next_run_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Business)
                .WithMany(b => b.ReportSchedules)
                .HasForeignKey(e => e.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.CreatedBy)
                .WithMany(u => u.CreatedSchedules)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.BusinessId);
            entity.HasIndex(e => new { e.BusinessId, e.IsActive });
        });
    }

    private static void ConfigureTask(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<geoStudio.Domain.Entities.Task>(entity =>
        {
            entity.ToTable("tasks");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.BusinessId).HasColumnName("business_id").IsRequired();
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id").IsRequired();
            entity.Property(e => e.AssignedToUserId).HasColumnName("assigned_to_user_id");
            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(500).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Priority).HasColumnName("priority").HasMaxLength(20).IsRequired();
            entity.Property(e => e.EffortEstimateHours).HasColumnName("effort_estimate_hours");
            entity.Property(e => e.DueDate).HasColumnName("due_date");
            entity.Property(e => e.RelatedGapId).HasColumnName("related_gap_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");

            entity.HasOne(e => e.Business)
                .WithMany(b => b.Tasks)
                .HasForeignKey(e => e.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.CreatedBy)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.AssignedTo)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(e => e.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.BusinessId);
            entity.HasIndex(e => e.AssignedToUserId);
            entity.HasIndex(e => e.Status);
        });
    }

    private static void ConfigureNotification(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("notifications");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            entity.Property(e => e.BusinessId).HasColumnName("business_id").IsRequired();
            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(500).IsRequired();
            entity.Property(e => e.Message).HasColumnName("message").IsRequired();
            entity.Property(e => e.NotificationType).HasColumnName("notification_type").HasMaxLength(100).IsRequired();
            entity.Property(e => e.RelatedEntityType).HasColumnName("related_entity_type").HasMaxLength(100);
            entity.Property(e => e.RelatedEntityId).HasColumnName("related_entity_id");
            entity.Property(e => e.IsRead).HasColumnName("is_read").HasDefaultValue(false);
            entity.Property(e => e.ReadAt).HasColumnName("read_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Business)
                .WithMany(b => b.Notifications)
                .HasForeignKey(e => e.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.IsRead);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => new { e.UserId, e.IsRead });
        });
    }

    private static void ConfigureNotificationPreference(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationPreference>(entity =>
        {
            entity.ToTable("notification_preferences");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            entity.Property(e => e.BusinessId).HasColumnName("business_id").IsRequired();
            entity.Property(e => e.AuditCompletedEmail).HasColumnName("audit_completed_email").HasDefaultValue(true);
            entity.Property(e => e.AuditCompletedInApp).HasColumnName("audit_completed_in_app").HasDefaultValue(true);
            entity.Property(e => e.GapFoundEmail).HasColumnName("gap_found_email").HasDefaultValue(true);
            entity.Property(e => e.GapFoundInApp).HasColumnName("gap_found_in_app").HasDefaultValue(true);
            entity.Property(e => e.GapFoundSlack).HasColumnName("gap_found_slack").HasDefaultValue(false);
            entity.Property(e => e.TaskAssignedEmail).HasColumnName("task_assigned_email").HasDefaultValue(true);
            entity.Property(e => e.TaskAssignedInApp).HasColumnName("task_assigned_in_app").HasDefaultValue(true);
            entity.Property(e => e.ReportReadyEmail).HasColumnName("report_ready_email").HasDefaultValue(true);
            entity.Property(e => e.ReportReadyInApp).HasColumnName("report_ready_in_app").HasDefaultValue(true);
            entity.Property(e => e.QuietHoursStart).HasColumnName("quiet_hours_start");
            entity.Property(e => e.QuietHoursEnd).HasColumnName("quiet_hours_end");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.User)
                .WithMany(u => u.NotificationPreferences)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Business)
                .WithMany(b => b.NotificationPreferences)
                .HasForeignKey(e => e.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.UserId, e.BusinessId }).IsUnique();
        });
    }

    private static void ConfigureAlertRule(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlertRule>(entity =>
        {
            entity.ToTable("alert_rules");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.BusinessId).HasColumnName("business_id").IsRequired();
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id").IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.ConditionType).HasColumnName("condition_type").HasMaxLength(100).IsRequired();
            entity.Property(e => e.ConditionValue).HasColumnName("condition_value").HasMaxLength(200).IsRequired();
            entity.Property(e => e.ConditionOperator).HasColumnName("condition_operator").HasMaxLength(20).IsRequired();
            entity.Property(e => e.NotificationMethod).HasColumnName("notification_method").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Frequency).HasColumnName("frequency").HasMaxLength(50).IsRequired();
            entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            entity.Property(e => e.LastTriggeredAt).HasColumnName("last_triggered_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Business)
                .WithMany(b => b.AlertRules)
                .HasForeignKey(e => e.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.CreatedBy)
                .WithMany()
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.BusinessId);
            entity.HasIndex(e => new { e.BusinessId, e.IsActive });
        });
    }

    private static void ConfigureAIProvider(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AIProvider>(entity =>
        {
            entity.ToTable("ai_providers");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            entity.Property(e => e.DisplayName).HasColumnName("display_name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.WebsiteUrl).HasColumnName("website_url").HasMaxLength(2048);
            entity.Property(e => e.ApiEndpoint).HasColumnName("api_endpoint").HasMaxLength(2048);
            entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");

            entity.HasIndex(e => e.Name).IsUnique();
        });
    }

    private static void ConfigureAIModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AIModel>(entity =>
        {
            entity.ToTable("ai_models");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.ProviderId).HasColumnName("provider_id").IsRequired();
            entity.Property(e => e.ModelName).HasColumnName("model_name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.ModelIdentifier).HasColumnName("model_identifier").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Version).HasColumnName("version").HasMaxLength(50);
            entity.Property(e => e.DisplayName).HasColumnName("display_name").HasMaxLength(200);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.CostPer1kInputTokens).HasColumnName("cost_per_1k_input_tokens").HasPrecision(10, 6);
            entity.Property(e => e.CostPer1kOutputTokens).HasColumnName("cost_per_1k_output_tokens").HasPrecision(10, 6);
            entity.Property(e => e.MaxTokens).HasColumnName("max_tokens");
            entity.Property(e => e.SupportsStreaming).HasColumnName("supports_streaming").HasDefaultValue(true);
            entity.Property(e => e.SupportsVision).HasColumnName("supports_vision").HasDefaultValue(false);
            entity.Property(e => e.IsOpenSource).HasColumnName("is_open_source").HasDefaultValue(false);
            entity.Property(e => e.ModelUrl).HasColumnName("model_url").HasMaxLength(2048);
            entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            entity.Property(e => e.IsAvailableForTesting).HasColumnName("is_available_for_testing").HasDefaultValue(true);
            entity.Property(e => e.LaunchDate).HasColumnName("launch_date");
            entity.Property(e => e.DeprecationDate).HasColumnName("deprecation_date");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Provider)
                .WithMany(p => p.Models)
                .HasForeignKey(e => e.ProviderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.ProviderId, e.ModelIdentifier }).IsUnique();
            entity.HasIndex(e => e.IsActive);
        });
    }

    private static void ConfigurePersona(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Persona>(entity =>
        {
            entity.ToTable("personas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.BusinessId).HasColumnName("business_id").IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsCustom).HasColumnName("is_custom").HasDefaultValue(false);
            entity.Property(e => e.IsPublic).HasColumnName("is_public").HasDefaultValue(true);
            entity.Property(e => e.AgeRange).HasColumnName("age_range").HasMaxLength(20);
            entity.Property(e => e.LocationType).HasColumnName("location_type").HasMaxLength(50);
            entity.Property(e => e.IncomeLevel).HasColumnName("income_level").HasMaxLength(50);
            entity.Property(e => e.EducationLevel).HasColumnName("education_level").HasMaxLength(50);
            entity.Property(e => e.TechSavviness).HasColumnName("tech_savviness").HasMaxLength(50);
            entity.Property(e => e.SearchIntent).HasColumnName("search_intent");
            entity.Property(e => e.PainPoints).HasColumnName("pain_points");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Business)
                .WithMany(b => b.Personas)
                .HasForeignKey(e => e.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.BusinessId);
            entity.HasIndex(e => new { e.BusinessId, e.IsPublic });
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedAt"))
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;

            if (entry.State == EntityState.Added && entry.Properties.Any(p => p.Metadata.Name == "CreatedAt"))
                entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
        }
    }
}
