namespace RockPapSci.Service.Common
{
    /// <summary>
    /// Exception for problems with third party services.
    /// </summary>
    public class ThirdServiceException: Exception
    {
        public ThirdServiceException(string? message) : base(message)
        {
        }

        public ThirdServiceException(string? message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
