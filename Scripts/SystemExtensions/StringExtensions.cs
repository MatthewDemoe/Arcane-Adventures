using System.Text.RegularExpressions;

namespace com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions
{
    public static class StringExtensions
    {
        public static string AddSpacesToPascalCase(this string pascalCase) => string.Join(" ", Regex.Split(pascalCase, @"(?<!^)(?=[A-Z](?![A-Z]|$))"));

        public static string CapitalizeFirstLetter(this string text) => text[0].ToString().ToUpper() + text.Substring(1);
    }
}