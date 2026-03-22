using PharmacyManagementSystem.Common.Rack;

namespace PharmacyManagementSystem.Server.Rack;

public interface IRackRepository
{
    Task<IReadOnlyCollection<Common.Rack.Rack>?> GetByFilterCriteriaAsync(RackFilter filter, CancellationToken cancellationToken);
    Task<Common.Rack.Rack?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.Rack.Rack?> AddAsync(Common.Rack.Rack? rack, CancellationToken cancellationToken);
    Task<Common.Rack.Rack?> UpdateAsync(Common.Rack.Rack? rack, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
