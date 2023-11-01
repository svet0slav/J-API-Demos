namespace TaxCalc.Service.Common
{
    /// <summary>
    /// Exceptions for the internal logic.
    /// </summary>
    public class ServiceException : Exception
    {
        public ServiceException(string? message) : base(message)
        {
        }

        public ServiceException(string? message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
