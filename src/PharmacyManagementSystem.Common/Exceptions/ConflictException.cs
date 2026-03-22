namespace PharmacyManagementSystem.Common.Exceptions;

/// <summary>
/// Represents a conflict error (HTTP 409).
/// </summary>
public class ConflictException : BaseException
{
    public ConflictException(string message) : base(message)
    {
    }
}
