using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DotNetHelper_HttpClient.Helpers
{
    public static class HttpContentHelper
    {

        /// <summary>
        /// Creates the content of the HTTP.
        /// </summary>
        /// <param name="json">json to attach to request body </param>
        /// <param name="type">The type.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <returns>HttpContent.</returns>
        public static HttpContent CreateStringContent(string value, string mediaType , Encoding encoding)
        {            
            return new StringContent(value, encoding, mediaType);
        }


        /// <summary>
        /// Creates the content of the HTTP.
        /// </summary>
        /// <param name="json">json to attach to request body </param>
        /// <returns>HttpContent.</returns>
        public static HttpContent CreateStringContent(string value)
        {
            return new StringContent(value);
        }

        /// <summary>
        /// Creates the content of the HTTP.
        /// </summary>
        /// <param name="json">json to attach to request body </param>
        /// <param name="type">The type.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <returns>HttpContent.</returns>
        public static ByteArrayContent CreateByteArrayContent(string value, string mediaType, Encoding encoding)
        {
            var buffer = encoding.GetBytes(value); // may need to only be serialize once
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            return byteContent;
        }

    }
}
