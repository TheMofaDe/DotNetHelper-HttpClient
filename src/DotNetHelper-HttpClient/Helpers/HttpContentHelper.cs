using System.IO;
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
        /// <param name="value"></param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="encoding"></param>
        /// <returns>HttpContent.</returns>
        public static StringContent CreateStringContent(string value, string mediaType , Encoding encoding)
        {            
            return new StringContent(value, encoding, mediaType);
        }


        /// <summary>
        /// Creates the content of the HTTP.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>HttpContent.</returns>
        public static StringContent CreateStringContent(string value)
        {
            return new StringContent(value);
        }

        /// <summary>
        /// Creates the content of the HTTP.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="encoding"></param>
        /// <returns>HttpContent.</returns>
        public static ByteArrayContent CreateByteArrayContent(string value, string mediaType, Encoding encoding)
        {
            var buffer = encoding.GetBytes(value); // may need to only be serialize once
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            return byteContent;
        }


        private static StreamContent CreateStreamContent(Stream stream)
        {
           return new StreamContent(stream);
        }

    }
}
