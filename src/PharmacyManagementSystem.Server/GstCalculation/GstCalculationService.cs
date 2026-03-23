using Microsoft.Extensions.Logging;

namespace PharmacyManagementSystem.Server.GstCalculation;

public class GstCalculationService(ILogger<GstCalculationService> logger) : IGstCalculationService
{
    private readonly ILogger<GstCalculationService> _logger = logger;

    public GstLineResult CalculateLineGst(
        decimal unitPrice,
        int quantity,
        decimal discountPercent,
        decimal gstRate,
        string? pharmacyGstin,
        string? patientGstin)
    {
        var grossValue = unitPrice * quantity;
        var discountAmount = grossValue * (discountPercent / 100m);
        var taxableValue = grossValue - discountAmount;

        var isInterState = IsInterState(pharmacyGstin, patientGstin);
        var gstFraction = gstRate / 100m;

        decimal cgst = 0m, sgst = 0m, igst = 0m;

        if (isInterState)
        {
            igst = Math.Round(taxableValue * gstFraction, 2);
        }
        else
        {
            cgst = Math.Round(taxableValue * (gstFraction / 2m), 2);
            sgst = cgst;
        }

        var totalAmount = taxableValue + cgst + sgst + igst;

        _logger.LogDebug(
            "GST line: taxable={Taxable}, cgst={Cgst}, sgst={Sgst}, igst={Igst}, interState={InterState}.",
            taxableValue, cgst, sgst, igst, isInterState);

        return new GstLineResult(taxableValue, cgst, sgst, igst, totalAmount);
    }

    private static bool IsInterState(string? pharmacyGstin, string? patientGstin)
    {
        if (string.IsNullOrWhiteSpace(pharmacyGstin) || string.IsNullOrWhiteSpace(patientGstin))
            return false;

        // First two characters of GSTIN are the state code
        var pharmacyState = pharmacyGstin[..2];
        var patientState = patientGstin[..2];

        return !string.Equals(pharmacyState, patientState, StringComparison.OrdinalIgnoreCase);
    }
}
