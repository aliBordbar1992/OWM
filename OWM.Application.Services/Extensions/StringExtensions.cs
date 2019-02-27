using System.Text.RegularExpressions;

namespace OWM.Application.Services.Extensions
{
    public static class StringExtensions
    {
        public static string CamelCaseToSentence(this string input)
        {
            return Regex.Replace(input, "[A-Z]", " $1", RegexOptions.Compiled).Trim();
        }
    }
}