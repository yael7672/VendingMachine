namespace VendingMachine.Api.Domain.Exceptions;

public class BusinessRuleException : Exception
{
    /// <summary>
    /// Creates an exception for a violated business rule.
    /// </summary>
    public BusinessRuleException(string message)
        : base(message)
    {
    }
}