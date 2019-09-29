using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetHelper_HttpClient.Extension
{
    internal static class ListExtension
    {
        /// <summary>
        /// Method Name Pretty Much Says It All
        /// </summary> 
        /// <param name="source"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this List<T> source, Func<T, bool> whereClause = null)
        {
            if (whereClause == null) return source == null || !source.Any();
            return source == null || !source.Any(whereClause);
        }
    }
}

