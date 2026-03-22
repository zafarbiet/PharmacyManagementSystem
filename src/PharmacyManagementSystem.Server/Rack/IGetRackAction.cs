using PharmacyManagementSystem.Common.Rack;

namespace PharmacyManagementSystem.Server.Rack;

public interface IGetRackAction
{
    Task<IReadOnlyCollection<Common.Rack.Rack>?> GetByFilterCriteriaAsync(RackFilter filter, CancellationToken cancellationToken);
    Task<Common.Rack.Rack?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
