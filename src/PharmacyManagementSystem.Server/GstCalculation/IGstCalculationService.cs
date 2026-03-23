namespace PharmacyManagementSystem.Server.GstCalculation;

public interface IGstCalculationService
{
    /// <summary>
    /// Computes GST breakdown for a single invoice line item.
    /// CGST + SGST apply for intra-state supply; IGST applies for inter-state.
    /// Inter-state is detected when the first two digits of patientGstin differ from pharmacyGstin.
    /// If either GSTIN is absent, intra-state is assumed.
    /// </summary>
    GstLineResult CalculateLineGst(
        decimal unitPrice,
        int quantity,
        decimal discountPercent,
        decimal gstRate,
        string? pharmacyGstin,
        string? patientGstin);
}
