

namespace Comma_Seperate_Value_Client
{
    /// <summary>
    /// An individual value of a CSV Document
    /// </summary>
    public class CSVValue
    {
        /// <summary>
        /// The name of the field.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The value of the field as a string.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Creates a new CSV Value.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value of the field.</param>
        public CSVValue(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public override string ToString() => $"{this.Name} '{this.Value}'";
        
    }
}
