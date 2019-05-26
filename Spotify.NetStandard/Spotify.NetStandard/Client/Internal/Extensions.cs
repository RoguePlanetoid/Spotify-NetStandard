using Spotify.NetStandard.Requests;
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
        #region Private Methods
        /// <summary>
        /// Get Property Description
        /// </summary>
        /// <param name="info">Property Info</param>
        /// <returns>Description as String</returns>
        private static string GetPropertyDescription(
            this PropertyInfo info)
        {
            return info.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .Cast<DescriptionAttribute>().Select(x => x.Description).FirstOrDefault();
        }

        /// <summary>
        /// From Bools
        /// </summary>
        /// <typeparam name="T">Source Type</typeparam>
        /// <param name="source">Type</param>
        /// <returns>String Array</returns>
        private static string[] FromBools<T>(
            T source)
        {
            List<string> results = null;
            if (source != null)
            {
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    if (info.CanRead && 
                        info.PropertyType == typeof(bool?))
                    {
                        object value = info.GetValue(source, null);
                        if (value != null && (bool)value)
                        {
                            if (results == null) results = new List<string>();
                            results.Add(info.GetPropertyDescription());
                        }
                    }
                }
            }
            return results?.ToArray();
        }
        #endregion Private Methods

        #region Public Methods
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
        public static string GetDescription<T>(
            this T value)
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
        /// Get QueryString As Dictionary
        /// </summary>
        /// <param name="querystring">Source QueryString</param>
        /// <returns>Dictionary of Key Values from QueryString</returns>
        public static Dictionary<string, string> QueryStringAsDictionary(
            this string querystring)
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(querystring);
            return nvc.AllKeys.ToDictionary(k => k, k => nvc[k]);
        }

        /// <summary>
        /// Get Scope
        /// </summary>
        /// <param name="scope">Scope</param>
        /// <returns>Results</returns>
        public static string Get(
            this Scope scope)
        {
            return FromBools(scope)?.AsDelimitedString();
        }

        /// <summary>
        /// Get Include Group
        /// </summary>
        /// <param name="includeGroup">Include Group Object</param>
        /// <returns>Results</returns>
        public static string[] Get(
            this IncludeGroup includeGroup)
        {
            return FromBools(includeGroup)?.ToArray();
        }

        /// <summary>
        /// Get Search Type
        /// </summary>
        /// <param name="searchType">Search Type Object</param>
        /// <returns>Results</returns>
        public static string[] Get(
            this SearchType searchType)
        {
            return FromBools(searchType)?.ToArray();
        }

        /// <summary>
        /// Set Parameter
        /// </summary>
        /// <param name="parameters">Parameters</param>
        /// <param name="prefix">Prefix</param>
        /// <param name="tuneableTrack">Tuneable Track Object</param>
        public static void SetParameter(
            this TuneableTrack tuneableTrack,
            Dictionary<string, string> parameters,
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
                            parameters.Add($"{prefix}_{info.GetDescription()}",
                            value.ToString());
                        }
                    }
                }
            }
        }
        #endregion Public Methods
    }
}
