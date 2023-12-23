
using System.Text;

namespace Comma_Seperate_Value_Client
{
    /// <summary>
    /// CSV Header
    /// </summary>
    public class CSVHeader
    {
        /// <summary>
        /// The safe name of the CSVHeader which will be used for conversion.
        /// </summary>
        public string Name { private set; get; }
        /// <summary>
        /// The display name of the header.
        /// </summary>
        public string DisplayName { private set; get; }

        /// <summary>
        /// Creates a CSV Header with the display name and derived safe name.
        /// </summary>
        /// <param name="displayName">Display name for the header.</param>
        public CSVHeader(string displayName)
        {
            this.DisplayName = displayName;
            this.Name = GenerateName(displayName);
        }

        /// <summary>
        /// Generates the safe name which will be used for conversions.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GenerateName(string value)
        {
            bool firstLetter = true;
            StringBuilder name = new StringBuilder();
            foreach(char currentCharacter in value)
            {
                if (currentCharacter == ' ')
                {
                    firstLetter = true;
                }
                else
                {
                    if (firstLetter)
                    {
                        name.Append(Char.ToUpper(currentCharacter));
                    }
                    else
                    {
                        name.Append(Char.ToLower(currentCharacter));
                    }
                }
            }
            return name.ToString();
        }
    }
}
