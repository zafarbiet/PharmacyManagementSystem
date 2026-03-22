namespace PharmacyManagementSystem.Server.Rack;

public interface ISaveRackAction
{
    Task<Common.Rack.Rack?> AddAsync(Common.Rack.Rack? rack, CancellationToken cancellationToken);
    Task<Common.Rack.Rack?> UpdateAsync(Common.Rack.Rack? rack, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
