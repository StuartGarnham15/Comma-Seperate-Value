

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

        /// <summary>
        /// Gets all values of the CSV Line as a list of strings.
        /// </summary>
        /// <returns>List of strings representing the line values.</returns>        
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

        /// <summary>
        /// Gets the value of the relevant field.
        /// </summary>
        /// <param name="field">Name of the field to get the value from.</param>
        /// <returns>The value of the field as a string.</returns>
        /// <exception cref="CSVException"></exception>
        public string GetValue(string field)
        {
            CSVValue? selectedValue = GetCSVValueByFieldName(field);
            if (selectedValue != null)
                return selectedValue.Value;
            throw new CSVException($"Could not find a field named '{field}'.");
        }

        /// <summary>
        /// Sets a new value to the relevant field.
        /// </summary>
        /// <param name="field">Name of the field to set.</param>
        /// <param name="newValue">The new value to set.</param>        
        public void SetValue(string field, string newValue)
        {
            CSVValue? selectedValue = GetCSVValueByFieldName(field);
            if (selectedValue != null)
            {
                selectedValue.Value = newValue;
            }
            else
            {
                throw new CSVException($"Could not find a field named '{field}'.");
            }
        }

        /// <summary>
        /// Get the CSVValue entry matching the name of the field if it exists.
        /// </summary>
        /// <param name="field">Name of the field to retrieve.</param>
        /// <returns>CSVValue corresponding to the field or null if it does not exist.</returns>
        private CSVValue? GetCSVValueByFieldName(string field)
        {
            CSVValue? selectedValue = null;
            foreach (var value in this.Values)
                if (value.Name.Equals(field, StringComparison.OrdinalIgnoreCase))
                    selectedValue = value;
            return selectedValue;
        }

    }
}
