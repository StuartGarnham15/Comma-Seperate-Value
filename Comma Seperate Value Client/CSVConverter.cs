using System.Data;

namespace Comma_Seperate_Value_Client
{
    /// <summary>
    /// Class used to convert from CSVDocument into various formats.
    /// </summary>
    public class CSVConverter
    {
        /// <summary>
        /// Converts a CSV Document into a DataTabke.
        /// </summary>
        /// <param name="document">CSVDocument to be converted.</param>
        /// <returns>A DataTable object containing the contents of the CSV Document.</returns>
        
        public DataTable ConvertToDataTable(CSVDocument document)
        {
            if (document.CSVHeaders != null)
            {
                DataTable dt = new DataTable();
                foreach (var header in document.CSVHeaders)
                    dt.Columns.Add(new DataColumn(header.Name));
                if (document.CSVLines != null)
                {
                    foreach (var line in document.CSVLines)
                    {
                        dt.Rows.Add(line.GetValues().ToArray());
                    }
                }
                return dt;
            }
            else
            {
                throw new CSVException("Cannot convert to DataTable with no CSV Headers");
            }
            
        }
    }
}

