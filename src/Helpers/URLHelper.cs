using DotNetHelper_HttpClient.Enum;
using DotNetHelper_HttpClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetHelper_HttpClient.Helpers
{
    public static class URLHelper
    {

        /// <summary>
        /// URLs the encode.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string UrlEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var reservedCharacters = "!*'();:@&=+$,/?%#[]"; 
            var sb = new StringBuilder();

            foreach (var @char in value)
            {
                if (reservedCharacters.IndexOf(@char) == -1)
                    sb.Append(@char);
                else
                    sb.AppendFormat("%{0:X2}", (int)@char);
            }
            return sb.ToString();
        }

        /// <summary>
        /// URLs the encode.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string UrlEscape(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return Uri.EscapeDataString(value);

        }


        /// <summary>
        /// Combines the baseurl with the endpoint and apply any url paramters to create a full address
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <returns>System.String.</returns>
        public static string CreateUrl(string baseurl, string resource, List<Parameter> headers)
        {

            baseurl = baseurl.EndsWith("/") ? baseurl.Remove(baseurl.Length - 1).Replace(" ", "") : baseurl.Replace(" ", "");
            if (!string.IsNullOrEmpty(resource))
            {
                resource = resource.StartsWith("/") ? resource.Replace(" ", "") : "/" + resource.Replace(" ", "");
                if (headers != null && headers.Count > 0)
                {
                    resource = resource.EndsWith("/") ? resource.Remove(resource.Length - 1) : resource;
                    resource = resource.Contains("?") ? resource : resource + "?";
                }
            }

            if (headers != null)
            {
                if (string.IsNullOrEmpty(resource)) resource = "";
                foreach (var param in headers.Where(p => p.Type == ParameterType.QueryString || p.Type == ParameterType.UrlSegment))
                {
                    var value = param.EscapeValue ? UrlEscape(param.Value.ToString()) : param.Value;
                    if (string.IsNullOrEmpty(param.Name) || string.IsNullOrEmpty(param.Value.ToString()))
                    {
                        continue;
                    }
                    if (resource.Contains("=") && resource.EndsWith("&"))
                    {
                        resource += $"{param.Name}={value}&";
                    }
                    else if (string.IsNullOrEmpty(resource))
                    {
                        baseurl = baseurl.Contains("?") ? baseurl : baseurl + "?";
                        resource += $"{param.Name}={value}&";
                    }
                    else
                    {
                        resource += $"&{param.Name}={value}&";
                    }

                }
                resource = resource.EndsWith("&") ? resource.Remove(resource.Length - 1) : resource;
            }

            if (resource != null && resource.Contains(" "))
            {

                // resource = UrlEscape(resource);
            }
            return baseurl + resource;
        }


    }
}
