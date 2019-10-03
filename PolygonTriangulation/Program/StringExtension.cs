using System.Text.RegularExpressions;

namespace Program
{
	public static class StringExtension
	{
		// Stolen from ZombieSheep, StackOverflow/questions/5796383
		/// <summary>
		/// Inserts spaces in camelcasing
		/// Eg. SomethingNice => Something Nice
		/// </summary>
		/// <param name="str">String to beautify.</param>
		/// <returns></returns>
		public static string Beautify(this string str)
		{
			return Regex.Replace(
				Regex.Replace(
					str,
					@"(\P{Ll})(\P{Ll}\p{Ll})",
					"$1 $2"
				),
				@"(\p{Ll})(\P{Ll})",
				"$1 $2"
			);
		}
	}
}
