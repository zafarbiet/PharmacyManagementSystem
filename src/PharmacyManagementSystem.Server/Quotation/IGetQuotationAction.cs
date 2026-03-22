using PharmacyManagementSystem.Common.Quotation;
namespace PharmacyManagementSystem.Server.Quotation;
public interface IGetQuotationAction
{
    Task<IReadOnlyCollection<Common.Quotation.Quotation>?> GetByFilterCriteriaAsync(QuotationFilter filter, CancellationToken cancellationToken);
    Task<Common.Quotation.Quotation?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
