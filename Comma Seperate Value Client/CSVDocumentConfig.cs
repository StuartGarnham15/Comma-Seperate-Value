
namespace Comma_Seperate_Value_Client
{
    /// <summary>
    /// A configuration that must be passed to the CSVDocument class in the constructor.
    /// </summary>
    public class CSVDocumentConfig
    {
        /// <summary>
        /// The character that seperates fields in the file. Defaults to comma.
        /// </summary>
        public char DelimitingCharacter { get; set; } = ',';
        /// <summary>
        /// If true the first line of the file should be the headers, if false then the data starts on the first line.
        /// </summary>
        public bool HasHeader { get; set; } = true;
    }
}
