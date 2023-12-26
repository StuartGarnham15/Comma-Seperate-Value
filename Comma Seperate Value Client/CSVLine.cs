
namespace Comma_Seperate_Value_Client
{
    /// <summary>
    /// CSV Line containing all the values.
    /// </summary>
    public class CSVLine
    {
        /// <summary>
        /// List of values.
        /// </summary>
        public List<CSVValue> Values { get; set; }

        /// <summary>
        /// Creates a new line complete with all the values and headers, if provided.
        /// If no headers are provided or the fields exceeds the number of headers
        /// then generic header names are made up based on the column index.
        /// </summary>
        /// <param name="values">A list of string values.</param>
        /// <param name="csvHeaders">An optional list of headers.</param>
        public CSVLine(List<string> values, List<CSVHeader>? csvHeaders)
        {
            this.Values = [];
            for(int index = 0; index < values.Count; index++)
            {
                string fieldName = $"Field{index}";
                if (csvHeaders != null)
                    if (csvHeaders.Count > index)
                        fieldName = csvHeaders[index].Name;
                this.Values.Add(new CSVValue(fieldName, values[index]));
            }
        }

        public List<string> GetValues()
        {
            if (this.Values != null)
            {
                List<string> values = new();
                foreach (CSVValue value in Values)
                    values.Add(value.Value);
                return values;
            }
            throw new CSVException("Cannot call GetValues on a CSVLine with no values.");

        }
    }
}
