namespace AutomateIt.Extensions.Extensions
{
    public static class StringExtensions
    {
        public static string CutFirst(this string s)
        {
            return s.Substring(1, s.Length - 1);
        }

        public static string CutFirst(this string s, char symbol)
        {
            return s.StartsWith(symbol.ToString()) ? s.Substring(1, s.Length - 1) : s;
        }

        public static string CutLast(this string s, char symbol)
        {
            return s.EndsWith(symbol.ToString()) ? s.Substring(0, s.Length - 1) : s;
        }

        public static int AsInt(this string s)
        {
            return int.Parse(FindInt(s));
        }

        public static decimal AsDecimal(this string s)
        {
            return decimal.Parse(s.FindNumber());
        }

        public static string FindNumber(this string s)
        {
            return RegexHelper.GetString(s, "((?:-|)\\d+(?:(?:\\.|,)\\d+)?)");
        }

        public static string FindInt(this string s)
        {
            return RegexHelper.GetString(s, "((?:-|)\\d+)");
        }

        public static string FindUInt(this string s)
        {
            return RegexHelper.GetString(s, "(\\d+)");
        }
    }
}