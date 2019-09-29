using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetHelper_HttpClient.Extension
{
    public static class StringExtension
    {
        public static string ReplaceLastOccurrence(this string source, string find, string replace, StringComparison comparison)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(find))
                return source;
            var place = source.LastIndexOf(find, comparison);
            if (place == -1)
                return source;
            source = source.Remove(place, find.Length).Insert(place, replace);
            return source;
        }

    }
}
