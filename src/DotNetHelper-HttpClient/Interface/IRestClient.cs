using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetHelper_HttpClient.Enum;

namespace DotNetHelper_HttpClient.Interface
{
    public interface IRestClient
    {
        /// <summary>
        /// execute get response as an asynchronous operation.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        Task<string> GetStringAsync(string url, Method method, HttpContent content = null);


        /// <summary>
        /// execute get response as an synchronous operation.
        /// </summary>
        /// <param name="url">The baseurl.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        string GetString(string url, Method method, HttpContent content = null);

        /// <summary>
        /// Execute get HTTP response as an asynchronous operation.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        Task<HttpResponseMessage> GetHttpResponseAsync(string url, Method method, HttpContent content = null);

        /// <summary>
        /// Execute get HTTP response as an synchronous operation.
        /// </summary>
        /// <param name="url">The baseurl.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        HttpResponseMessage GetHttpResponse(string url, Method method, HttpContent content = null);


        /// <summary>
        /// Execute get stream as an asynchronous operation.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;Stream&gt;.</returns>
        Task<Stream> GetStreamAsync(string url, Method method, HttpContent content = null);


        /// <summary>
        /// Execute get stream as an synchronous operation.
        /// </summary>
        /// <param name="url">The baseurl.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;Stream&gt;.</returns>
        Stream GetStream(string url, Method method, HttpContent content = null);

        /// <summary>
        /// Execute get bytes as an asynchronous operation.
        /// </summary>
        /// <param name="url">The baseurl.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        Task<byte[]> GetBytesAsync(string url, Method method, HttpContent content = null);

        /// <summary>
        /// Execute get bytes as an synchronous operation.
        /// </summary>
        /// <param name="url">The baseurl.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        byte[] GetBytes(string url, Method method, HttpContent content = null);


        /// <summary>
        /// execute get type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="url">The url.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        Task<T> GetAsync<T>(Func<string, T> deserializer, string url, Method method, HttpContent content = null);

        /// <summary>
        /// execute get type as an synchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="url">The url.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        T Get<T>(Func<string, T> deserializer, string url, Method method, HttpContent content = null);

        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="fileStream"></param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        Task<bool> DownloadFileAsync(string url, FileStream fileStream);

        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="fileStream"></param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        void DownloadFile(string url, FileStream fileStream);

    }
}