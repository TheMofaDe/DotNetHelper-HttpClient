using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetHelper_HttpClient.Enum
{

    /// <summary>
    /// Types of parameters that can be added to requests
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// The cookie
        /// </summary>
        Cookie,
        /// <summary>
        /// The get or post
        /// </summary>
        /// GetOrPost,
        /// <summary>
        /// The URL segment
        /// </summary>
        UrlSegment,
        /// <summary>
        /// The HTTP header
        /// </summary>
        HttpHeader,
        /// <summary>
        /// The request body
        /// </summary>
        /// RequestBody,


        /// <summary>
        /// The query string
        /// </summary>
        QueryString
    }
}
