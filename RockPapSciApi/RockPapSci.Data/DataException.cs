namespace RockPapSci.Data
{
    /// <summary>
    /// Exceptions for the internal data problems.
    /// </summary>
    public class DataException : Exception
    {
        public DataException(string? message) : base(message)
        {
        }

        public DataException(string? message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
