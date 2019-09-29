using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DotNetHelper_HttpClient.Enum;

namespace DotNetHelper_HttpClient.Extension
{
    public static class MethodExtension
    {

        public static HttpMethod MapToHttpMethod(this Method method)
        {

            switch (method)
            {
                case Method.Get:
                    return HttpMethod.Get;
                case Method.Post:
                    return HttpMethod.Post;
                case Method.Put:
                    return HttpMethod.Put;
                case Method.Delete:
                    return HttpMethod.Delete;
                //case Method.Send:

                //    return HttpMethod.;
                case Method.Head:
                    return HttpMethod.Head;
                case Method.Option:
                    return HttpMethod.Options;
                case Method.Trace:
                    return HttpMethod.Trace;
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }
        }

    }
}
