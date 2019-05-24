namespace AutomateIt.Extensions
{
    using System.Text.RegularExpressions;

    public static class RegexHelper
    {
        public static int GetInt(string text, string pattern, string group = null)
        {
            return int.Parse(GetString(text, pattern, group));
        }

        public static string GetString(string text, string pattern, string group = null)
        {
            var match = new Regex(pattern).Match(text);
            return group == null ? match.Groups[1].Value : match.Groups[group].Value;
        }

        public static bool IsMatch(string text, string pattern)
        {
            return new Regex(pattern).Match(text).Success;
        }
    }
}