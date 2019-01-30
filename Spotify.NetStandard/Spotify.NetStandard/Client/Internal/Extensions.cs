using Spotify.NetStandard.Enums;
using Spotify.NetStandard.Responses;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Spotify.NetStandard.Client.Internal
{
    /// <summary>
    /// Extension Methods
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Get Dictionary Value or Default
        /// </summary>
        /// <typeparam name="TKey">Key Type</typeparam>
        /// <typeparam name="TValue">Value Type</typeparam>
        /// <param name="dictionary">Dictionary</param>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary, 
            TKey key, TValue defaultValue = default(TValue))
        {
            if (dictionary == null) { throw new ArgumentNullException(nameof(dictionary)); }
            if (key == null) { throw new ArgumentNullException(nameof(key)); }
            return dictionary.TryGetValue(key, out TValue value) ? value : defaultValue;
        }

        /// <summary>
        /// Get Description
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="value">Value</param>
        /// <returns>Description as String</returns>
        public static string GetDescription<T>(this T value)
        {
            FieldInfo field = typeof(T).GetField(value.ToString());
            return field.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .Cast<DescriptionAttribute>().Select(x => x.Description).FirstOrDefault();
        }

        /// <summary>
        /// Get Array of String as Delimited String
        /// </summary>
        /// <param name="items">Array of Strings</param>
        /// <returns>Comma delimited String of Strings</returns>
        public static string AsDelimitedString(this string[] items)
        {
            return string.Join(",", items);
        }

        /// <summary>
        /// Get Scopes from Enum As Delimited String
        /// </summary>
        /// <param name="scopes">ScopeType Array</param>
        /// <returns>Comma delimited Scope values</returns>
        public static string AsDelimitedString(this ScopeType[] scopes)
        {
            if (scopes == null) return string.Empty;
            string[] results = scopes.Select(f => f.GetDescription()).ToArray();
            return results.AsDelimitedString();
        }

        /// <summary>
        /// Get QueryString As Dictionary
        /// </summary>
        /// <param name="querystring">Source QueryString</param>
        /// <returns>Dictionary of Key Values from QueryString</returns>
        public static Dictionary<string, string> QueryStringAsDictionary(this string querystring)
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(querystring);
            return nvc.AllKeys.ToDictionary(k => k, k => nvc[k]);
        }

        /// <summary>
        /// Set Tuneable Track
        /// </summary>
        /// <param name="results">Results</param>
        /// <param name="prefix">Prefix</param>
        /// <param name="tuneableTrack">Tuneable Track Object</param>
        public static void Set(
            this TuneableTrack tuneableTrack,
            Dictionary<string, string> results,
            string prefix)
        {
            if (tuneableTrack != null)
            {
                foreach (PropertyInfo info in typeof(TuneableTrack).GetProperties())
                {
                    if (info.CanRead)
                    {
                        object value = info.GetValue(tuneableTrack, null);
                        if (value != null)
                        {
                            results.Add($"{prefix}_{GetDescription(value)}",
                            value.ToString());
                        }
                    }
                }
            }
        }
    }
}
