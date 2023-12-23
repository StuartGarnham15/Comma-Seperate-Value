

namespace Comma_Seperate_Value_Client
{
    /// <summary>
    /// Exception class specific to the CSV Document
    /// </summary>
    public class CSVException : Exception
    {
        /// <summary>
        /// Creates a CSVException with message text and optional inner exception.
        /// </summary>
        /// <param name="message">Error text.</param>
        /// <param name="innerException">Exception that was caught and triggered this exception.</param>
        public CSVException(string message, Exception? innerException = null) 
            : base(message, innerException) { }
    }
}
