using System.Text;

namespace Comma_Seperate_Value_Client
{
    /// <summary>
    /// Support class used by CSVDocument to write it's contents to a file.
    /// </summary>
    internal class CSVWriter : IDisposable
    {
        private bool _disposed;
        private StreamWriter _writer;        
        private char _delimiter;

        /// <summary>
        /// Creates a new CSVWriter with the given file name and delimiter.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="delimiter"></param>
        internal CSVWriter(string fileName, char delimiter)
        {
            _writer = new StreamWriter(fileName);
            _delimiter = delimiter;
        }       

        /// <summary>
        /// Writes the header displays names to a file seperated by the delimiter.
        /// </summary>
        /// <param name="csvHeaders">List of headers from the CSVDocument.</param>
        internal void WriteHeaders(List<CSVHeader> csvHeaders)
        {
            List<string> values = [];
            foreach(CSVHeader header in csvHeaders)
            {
                values.Add(CSVFriendlyString(header.DisplayName));
            }
            _writer.WriteLine(string.Join(_delimiter, values));
        }

        /// <summary>
        /// Writes all values from a CSVLine to a file seperated by the delimiter.
        /// </summary>
        /// <param name="line">Line from the CSVDocument.</param>
        internal void WriteLine(CSVLine line)
        {
            List<string> values = [];
            foreach(CSVValue csvValue in line.Values)
            {
                values.Add(CSVFriendlyString(csvValue.Value));
            }
            _writer.WriteLine(string.Join(_delimiter, values));
        }        

        /// <summary>
        /// Closes the underling writer.
        /// </summary>
        internal void Close()
        {
            _writer.Close();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _writer.Dispose();
                }
                
                _disposed = true;
            }
        }        

        /// <summary>
        /// Disposes the CSVWriter object
        /// </summary>
        public void Dispose()
        {            
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Checks to see if the value contains double quotes, carriage return/new line chars or the delimiter
        /// and wraps the value in quotes. If there are double quotes within the value these will be escaped by
        /// changing them to double double quotes.
        /// </summary>
        /// <param name="value">The text of the value to be checked.</param>
        /// <returns></returns>
        private string CSVFriendlyString(string value)
        {
            if (!value.Contains('"') && !value.Contains(_delimiter) && !value.Contains('\r') && !value.Contains('\n'))
                return value;
            return $"\"{value.Replace("\"", "\"\"")} \"";
        }
    }
}
