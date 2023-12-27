using System.Text;

namespace Comma_Seperate_Value_Client
{
    internal class CSVWriter : IDisposable
    {        
        private StreamWriter _writer;
        private bool _disposed;

        internal CSVWriter(string fileName)
        {
            _writer = new StreamWriter(fileName);
        }       

        internal void WriteHeaders(List<CSVHeader> cSVHeaders)
        {
            List<string> values = [];
            foreach(CSVHeader header in cSVHeaders)
            {
                values.Add(CSVFriendlyString(header.DisplayName));
            }
            _writer.WriteLine(string.Join(',', values));
        }

        internal void WriteLine(CSVLine line)
        {
            List<string> values = [];
            foreach(CSVValue csvValue in line.Values)
            {
                values.Add(CSVFriendlyString(csvValue.Value));
            }
            _writer.WriteLine(string.Join(',', values));
        }        

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

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private static string CSVFriendlyString(string value)
        {
            if (!value.Contains('"') && !value.Contains(',') && !value.Contains('\r') && !value.Contains('\n'))
                return value;
            return $"\"{value.Replace("\"", "\"\"")} \"";
        }
    }
}
