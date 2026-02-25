using geoStudio.Application.DTOs;

namespace geoStudio.Application.Interfaces;

/// <summary>Business profile management service contract.</summary>
public interface IBusinessService
{
    Task<BusinessResponse> CreateAsync(Guid ownerUserId, CreateBusinessRequest request, CancellationToken cancellationToken = default);
    Task<BusinessResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BusinessResponse>> GetByOwnerAsync(Guid ownerUserId, CancellationToken cancellationToken = default);
    Task<BusinessResponse> UpdateAsync(Guid id, UpdateBusinessRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
