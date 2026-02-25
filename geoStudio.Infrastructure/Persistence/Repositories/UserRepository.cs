using geoStudio.Application.Interfaces;
using geoStudio.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace geoStudio.Infrastructure.Persistence.Repositories;

/// <summary>Repository for User entity with domain-specific queries.</summary>
public class UserRepository(GeoStudioDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await DbSet.FirstOrDefaultAsync(u => u.Email == email.ToLower(), cancellationToken);

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        => await DbSet.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        => await DbSet.AnyAsync(u => u.Email == email.ToLower(), cancellationToken);
}
