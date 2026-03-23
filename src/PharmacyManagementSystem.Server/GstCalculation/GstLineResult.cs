namespace PharmacyManagementSystem.Server.GstCalculation;

public record GstLineResult(
    decimal TaxableValue,
    decimal CgstAmount,
    decimal SgstAmount,
    decimal IgstAmount,
    decimal TotalAmount);
