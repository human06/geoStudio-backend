# ── Build stage ──────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY geoStudio.Domain/geoStudio.Domain.csproj             geoStudio.Domain/
COPY geoStudio.Application/geoStudio.Application.csproj   geoStudio.Application/
COPY geoStudio.Infrastructure/geoStudio.Infrastructure.csproj geoStudio.Infrastructure/
COPY geoStudio.BackgroundJobs/geoStudio.BackgroundJobs.csproj geoStudio.BackgroundJobs/
COPY geoStudio.API/geoStudio.API.csproj                   geoStudio.API/

RUN dotnet restore geoStudio.API/geoStudio.API.csproj

COPY . .
RUN dotnet publish geoStudio.API/geoStudio.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ── Runtime stage ─────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "geoStudio.API.dll"]
