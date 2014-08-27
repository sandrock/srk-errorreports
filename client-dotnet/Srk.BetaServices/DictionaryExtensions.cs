
namespace Srk.BetaServices
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    /// <summary>
    /// Extension methods for dictionaries.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Constructs a QueryString (string).
        /// Consider this method to be the opposite of "System.Web.HttpUtility.ParseQueryString"
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns>String</returns>
        public static string GetQueryString(this Dictionary<string, string> dictionary)
        {
            var items = new List<string>();

            foreach (var item in dictionary)
            {
                items.Add(string.Concat(item.Key, "=", HttpUtilityEx.UrlEncode(item.Value)));
            }

            return string.Join("&", items.ToArray());
        }
    }
}
