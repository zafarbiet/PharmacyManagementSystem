using PharmacyManagementSystem.Common.Quotation;
namespace PharmacyManagementSystem.Server.Quotation;
public interface IQuotationRepository
{
    Task<IReadOnlyCollection<Common.Quotation.Quotation>?> GetByFilterCriteriaAsync(QuotationFilter filter, CancellationToken cancellationToken);
    Task<Common.Quotation.Quotation?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.Quotation.Quotation?> AddAsync(Common.Quotation.Quotation? quotation, CancellationToken cancellationToken);
    Task<Common.Quotation.Quotation?> UpdateAsync(Common.Quotation.Quotation? quotation, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
