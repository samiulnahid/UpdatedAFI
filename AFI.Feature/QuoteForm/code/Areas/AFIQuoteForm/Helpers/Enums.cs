using System;
using System.Linq;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers
{
    public static class Enums
    {
        public enum UI_EligibilityStatus
        {
            military = 0,
            spouse = 1,
            dod = 2,
            child = 3,
            nonMilitary = 4,
            currentMember = 5,
			va = 6,
			parent=7,
			auxiliary = 8
		}

        public enum DB_EligibilityStatus
        {
            Military = 0,
            MilitarySpouse = 1,
            Civilian = 2,
            Child = 3,
            NotEligibleCivilian = 4,
            CurrentMember = 5,
			VA = 6,
			Parent = 7,
			Auxiliary = 8
		}

        /// <summary>
        /// _ => space
        /// <para>_slash_ => /</para>
        /// <para>_dash_ => -</para>
        /// <para>_popen_ => (</para>
        /// <para>_pclose_ => )</para>
        /// </summary>
        public static string NormalizeEnumValues(this string s)
		{
			return s.Replace("_pclose_", ")")
			.Replace("_popen_", "(")
			.Replace("_slash_", "/")
			.Replace("_dash_", "-")
			.Replace("_dblquote", "\"")
			.Replace("_prcnt_", "%")
			.Replace("_quote_", "'")
			.Replace("_dot_", ".")
			.Replace("_dollar_", "$")
			.Replace("_amp_", "&")
			.Replace("_comma_", ",")
			.Replace("_", " ")

			.Trim();
		}

		/// <summary>
		/// space => _
		/// <para>leading number => _leading number</para>
		/// <para>) => _pclose_</para>
		/// <para>( => _popen_</para>
		/// <para>- => _dash_</para>
		/// <para>/ => _slash_</para>
		/// </summary>
		public static string ReverseNormalizedEnumValue(this string s)
		{
			if (s == null) s = "";
			if (s.Length > 0 && "0123456789".Contains(s[0])) s = "_" + s;
			s = s.Replace(")", "_pclose_")
			.Replace("(", "_popen_")
			.Replace("/", "_slash_")
			.Replace("-", "_dash_")
			.Replace("\"", "_dblquote_")
			.Replace("%", "_prcnt_")
			.Replace("'", "_quote_")
			.Replace(".", "_dot_")
			.Replace("$", "_dollar_")
			.Replace("&", "_amp_")
			.Replace(",", "_comma_")
			.Replace(" ", "_")
			.Trim();
			if (s.Length == 0) s = "_";
			return s;
		}

		/// <summary>T must be an enum type
		/// <para>space => _</para>
		/// <para>leading number => _leading number</para>
		/// <para>) => _pclose_</para>
		/// <para>( => _popen_</para>
		/// <para>- => _dash_</para>
		/// <para>/ => _slash_</para>
		/// </summary>
		/// <typeparam name="T">must be an enum type</typeparam>
		public static T ReverseNormalizeEnum<T>(this string s)
		{
			Type t = typeof(T);
			if (!t.IsEnum) throw new ArgumentException("Type t must be an enum type", "t");
			string e = s.ReverseNormalizedEnumValue();
			return (T)Enum.Parse(t, e, true);
		}

		/// <summary>
		/// Converts one enum into another
		/// </summary>
		public static T To<T>(this Enum e)
		{
			Type t = typeof(T);
			if (!t.IsEnum) throw new ArgumentException("Type t must be an enum type", "t");
			try
			{
				return (T)Enum.ToObject(t, Convert.ToUInt32(e));
			}
			catch
			{
				return default(T);
			}
		}

		public static T ToEnum<T>(this string s)
		{
			Type t = typeof(T);
			if (!t.IsEnum) throw new ArgumentException("Type t must be an enum type", "t");
			return (T)Enum.Parse(t, s, true);
		}
	}
}