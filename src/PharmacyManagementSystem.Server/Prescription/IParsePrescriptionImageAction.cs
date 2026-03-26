namespace PharmacyManagementSystem.Server.Prescription;

public interface IParsePrescriptionImageAction
{
    /// <summary>
    /// Sends a prescription image to the Claude Vision API and returns
    /// the list of drug / medicine names extracted from it.
    /// Returns an empty list when nothing can be identified.
    /// </summary>
    Task<IReadOnlyList<string>> ParseAsync(
        Stream imageStream,
        string contentType,
        CancellationToken cancellationToken);
}
