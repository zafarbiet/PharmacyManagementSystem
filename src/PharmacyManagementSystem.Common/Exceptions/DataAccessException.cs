namespace PharmacyManagementSystem.Common.Exceptions;

/// <summary>
/// Represents a data access error (HTTP 500).
/// </summary>
public class DataAccessException : BaseException
{
    public DataAccessException(string message) : base(message)
    {
    }
}
