namespace PharmacyManagementSystem.Common.Exceptions;

/// <summary>
/// Represents a bad request error (HTTP 422).
/// </summary>
public class BadRequestException : BaseException
{
    public BadRequestException(string message) : base(message)
    {
    }
}
