using System;
using System.Text.RegularExpressions;

namespace AFI.Feature.Quote.Helpers
{
    public static class Strings
    {
        public static string DigitsOnly(this string value)
        {
            string alt = Regex.Replace(value, "[^0-9]", String.Empty);
            return alt;
        }
    }
}