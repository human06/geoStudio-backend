namespace geoStudio.Infrastructure.Caching;

/// <summary>Abstraction over a distributed cache (Redis).</summary>
public interface ICacheService
{
    /// <summary>Retrieves a cached value by key. Returns <c>default</c> on miss or error.</summary>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    /// <summary>Stores a value under <paramref name="key"/> with an optional expiry.</summary>
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default);

    /// <summary>Removes a cached entry.</summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>Returns <c>true</c> when a non-null value exists for <paramref name="key"/>.</summary>
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
}
