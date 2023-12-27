using System.Text;

namespace Comma_Seperate_Value_Client
{
    /// <summary>
    /// Creates a CSV Document object which can be used to read and write CSV files
    /// as well as serializing CSV data into a class.
    /// </summary>
    /// <param name="configuration">
    /// This configures the behaviour of the CSV class by setting the character
    /// to use for delimiting, a comma by default, and whether the first line
    /// is a list of headers.
    /// </param>
    public class CSVDocument(CSVDocumentConfig configuration)
    {
        /// <summary>
        /// The character to be used as the delimiter to seperate the fields.
        /// </summary>
        public char DelimitingCharacter { private set; get; } = configuration.DelimitingCharacter;
        /// <summary>
        /// If set to 'true' then the first line of the CSV file will be used to set the headers.
        /// </summary>
        public bool HasHeader { private set; get; } = configuration.HasHeader;
        /// <summary>
        /// The current file to be read from or written to.
        /// </summary>
        public string? FileName { private set; get; }
        /// <summary>
        /// List of CSV headers.
        /// </summary>
        public List<CSVHeader>? CSVHeaders { private set; get; }
        /// <summary>
        /// List of CSV Lines. All values are held as strings so will need conversion to the desired variable types when reading.
        /// </summary>
        public List<CSVLine>? CSVLines { private set; get; }

        /// <summary>
        /// Attempts to load the supplied file and read it's contents.
        /// </summary>
        /// <param name="fileName">Location of the file to read.</param>        
        public void LoadFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    this.FileName = fileName;
                    string contents = File.ReadAllText(fileName);
                    ConvertStringToCSVDocument(contents);
                }
                catch (Exception ex)
                {
                    throw new CSVException($"The file '{fileName}' was found but an issue occurred loading it.", ex);
                }
            }
            else
            {
                throw new CSVException($"The file name '{fileName}' could not be found!");
            }
        }

        /// <summary>
        /// Saves the contents of the CSVDocument into a file.
        /// </summary>
        /// <param name="fileName">The full path of the file to save.</param>
        /// <param name="allowOverwrite">If set to false an exception will occur if the file already exists.</param>        
        public void SaveToFile(string fileName, bool allowOverwrite = false)
        {
            if (File.Exists(fileName) && !allowOverwrite)
                throw new CSVException($"A file with the name '{fileName}' already exists. Choose a new file name or specify allowOverwrite to be true.");
            if (this.CSVLines != null)
            {
                try
                {
                    using CSVWriter writer = new(fileName, this.DelimitingCharacter);
                    if (this.HasHeader)
                        writer.WriteHeaders(this.CSVHeaders);
                    foreach (var line in this.CSVLines)
                    {
                        writer.WriteLine(line);
                    }
                    writer.Close();
                }
                catch (CSVException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new CSVException("An issue occurred when attempting to save the file.", ex);
                }
            }
            else
            {
                throw new CSVException($"Cannot write CSV file with no CSV Lines.");
            }
        }

        

        /// <summary>
        /// Splits the contents down into lines based on the denominator.
        /// </summary>
        /// <param name="contents">CSV file respresented by a string.</param>
        private void ConvertStringToCSVDocument(string contents)
        {
            List<string> csvLines = SplitContentsDownToLines(contents);            
            ConvertSplitLinesToCSVDocument(csvLines);
        }        

        /// <summary>
        /// Splits the CSV content into seperate lines by finding new line/carriage return in the string as long
        /// as it is not inside quotes.
        /// </summary>
        /// <param name="contents">The raw line text.</param>
        /// <returns>A list of CSV lines.</returns>
        private List<string> SplitContentsDownToLines(string contents)
        {
            List<string> splitLines = [];
            StringBuilder? currentLine = null;
            char? lastCharacter = null;
            bool insideQuotes = false;
            foreach(char currentCharacter in contents)
            {
                switch(currentCharacter)
                {
                    case '\n':
                    case '\r':                        
                        if (insideQuotes)
                        {
                            currentLine ??= new();
                            currentLine.Append(currentCharacter);                            
                        }
                        else
                        {
                            if (currentLine != null)
                            {
                                splitLines.Add(currentLine.ToString());
                                currentLine = null;
                            }
                        }
                        lastCharacter = currentCharacter;
                        break;
                    case '"':
                        if (lastCharacter == '"')
                        {
                            currentLine ??= new();
                            currentLine.Append(currentCharacter);
                            insideQuotes = !insideQuotes;
                            lastCharacter = '~';
                        }
                        else
                        {
                            currentLine ??= new();
                            currentLine.Append(currentCharacter);
                            insideQuotes = !insideQuotes;
                            lastCharacter = currentCharacter;
                        }                        
                        break;
                    //case char value when value == this.DelimitingCharacter:                        
                    default:
                        currentLine ??= new();
                        currentLine.Append(currentCharacter);
                        lastCharacter = currentCharacter;
                        break;
                }
                
            }
            if (currentLine != null)
                splitLines.Add(currentLine.ToString());
            return splitLines;
        }        

        /// <summary>
        /// Iterates through all the csv lines, sets the headers if appropriates and then adds the remaining lines
        /// into structured rows.
        /// </summary>
        /// <param name="csvLines">Lines of CSV text.</param>
        private void ConvertSplitLinesToCSVDocument(List<string> csvLines)
        {
            if (this.HasHeader)
            {
                AddCSVHeaders(csvLines[0]);
                csvLines.RemoveAt(0);
            }
            this.CSVLines = new();
            foreach(var line in csvLines)
            {
                AddCSVRow(line);
            }            
        }

        /// <summary>
        /// Adds the line of text to the CSV Document as a structured CSV Row.
        /// </summary>
        /// <param name="lineString">Line of text containing the values to add as a row.</param>
        private void AddCSVRow(string lineString)
        {
            List<string> values = SplitCSVLine(lineString);
            this.CSVLines!.Add(new CSVLine(values, this.CSVHeaders));
        }

        /// <summary>
        /// Adds headers to the CSV Document.
        /// </summary>
        /// <param name="headerString">Line of text containing the headers.</param>
        private void AddCSVHeaders(string headerString)
        {
            this.CSVHeaders = new List<CSVHeader>();
            List<string> names = SplitCSVLine(headerString);
            foreach(var name in names)        
                this.CSVHeaders.Add(new CSVHeader(name));            
        }

        /// <summary>
        /// Parses the line of text and breaks it down into seperate values.
        /// </summary>
        /// <param name="text">The line of CSV text to be parsed.</param>
        /// <returns>A list of values taken from the text.</returns>
        private List<string> SplitCSVLine(string text)
        {
            List<string> splitLines = [];
            StringBuilder currentValue = new();
            char? lastCharacter = null;
            bool insideQuotes = false;
            foreach(char currentCharacter in text)
            {
                switch(currentCharacter)
                {                    
                    case '"':
                        if (lastCharacter == currentCharacter)
                        {
                            currentValue.Append(currentCharacter);
                            insideQuotes = !insideQuotes;
                        }
                        else
                        {
                            insideQuotes = !insideQuotes;
                        }
                        break;
                    case char value when value == this.DelimitingCharacter:
                        if (insideQuotes)
                        {
                            currentValue.Append(currentCharacter);
                        }
                        else
                        {
                            splitLines.Add(currentValue.ToString());
                            currentValue = new();
                        }
                        break;
                    default:                        
                        currentValue.Append(currentCharacter);
                        break;
                }
                lastCharacter = currentCharacter;
            }
            splitLines.Add(currentValue.ToString());
            return splitLines;
        }
    }


}
