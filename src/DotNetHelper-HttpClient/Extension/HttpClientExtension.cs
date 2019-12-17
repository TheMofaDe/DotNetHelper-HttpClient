using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DotNetHelper_HttpClient.Enum;
using DotNetHelper_HttpClient.Helpers;
using DotNetHelper_HttpClient.Models;

namespace DotNetHelper_HttpClient.Extension
{
    public static class HttpClientExtension
    {

        public static bool AlwaysEnsureSuccessCode { get; set; }


        /// <summary>
        /// Sends a HttpRequest and returns a reponse as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers"></param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        private static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, Method method, string url, List<Parameter> headers, HttpContent content = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Options, url)
            {
                Content = content
               ,
                Method = method.MapToHttpMethod()
            };
            if (!headers.IsNullOrEmpty())
            {
                headers.Where(p => p.Type == ParameterType.HttpHeader).ToList().ForEach(delegate (Parameter parameter)
                {
                    request.Headers.Add(parameter.Name, parameter.Value.ToString());
                });

                var cookies = headers.Where(p => p.Type == ParameterType.Cookie).Select(c => $"{c.Name}={c.Value}; ");
                if (cookies.Any())
                {
                    var value = string.Join(string.Empty, cookies);
                    value = value.ReplaceLastOccurrence("; ", string.Empty, StringComparison.OrdinalIgnoreCase);
                    request.Headers.Add("Cookie", value);
                }

            }
            return await httpClient.SendAsync(request);
        }


        /// <summary>
        /// do work as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers"></param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        private static HttpResponseMessage Send(this HttpClient httpClient, Method method, string url, List<Parameter> headers, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => SendAsync(httpClient,method, url, headers, content));
        }


        #region GetString


        /// <summary>
        /// execute get response as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url">The url.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public static async Task<string> GetStringAsync(this HttpClient httpClient, string url, Method method, HttpContent content = null)
        {
            return await GetStringAsync(httpClient,url, null, null, Method.Get, null);
        }

        /// <summary>
        /// execute get response as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient"></param> 
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public static async Task<string> GetStringAsync(this HttpClient httpClient, string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            baseurl.IsNullThrow(nameof(baseurl));
            var url = URLHelper.CreateUrl(baseurl, resource, headers);

            var response = await SendAsync(httpClient,method, url, headers, content);
            EnsureSuccessCodeAsync(response);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }


        /// <summary>
        /// execute get response as an synchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public static string GetString(this HttpClient httpClient, string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => GetStringAsync(httpClient,baseurl, resource, headers, method, content));
        }

        /// <summary>
        /// execute get response as an synchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url">The baseurl.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public static string GetString(this HttpClient httpClient, string url, Method method, HttpContent content = null)
        {
            return GetString(httpClient,url, null, null, method, content);
        }



        #endregion

        #region GetHttpResponse

        /// <summary>
        /// Execute get HTTP response as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url">The url.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        public static async Task<HttpResponseMessage> GetHttpResponseAsync(this HttpClient httpClient,string url, Method method, HttpContent content = null)
        {
            return await GetHttpResponseAsync(httpClient,url, null, null, method, content);
        }

        /// <summary>
        /// Execute get HTTP response as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        public static async Task<HttpResponseMessage> GetHttpResponseAsync(this HttpClient httpClient, string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            baseurl.IsNullThrow(nameof(baseurl));
            var url = URLHelper.CreateUrl(baseurl, resource, headers);

            var response = await SendAsync(httpClient,method, url, headers, content);
            EnsureSuccessCodeAsync(response);
            return response;
        }



        /// <summary>
        /// Execute get HTTP response as an synchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url">The baseurl.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        public static HttpResponseMessage GetHttpResponse(this HttpClient httpClient, string url, Method method, HttpContent content = null)
        {
            return GetHttpResponse(httpClient,url, null, null, method, content);
        }

        /// <summary>
        /// Execute get HTTP response as an synchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        public static HttpResponseMessage GetHttpResponse(this HttpClient httpClient, string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => GetHttpResponseAsync(httpClient,baseurl, resource, headers, method, content));
        }


        #endregion

        #region GetStream


        /// <summary>
        /// Execute get stream as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url">The url.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;Stream&gt;.</returns>
        public static async Task<Stream> GetStreamAsync(this HttpClient httpClient, string url, Method method, HttpContent content = null)
        {
            return await GetStreamAsync(httpClient,url, null, null, method, content);
        }


        /// <summary>
        /// Execute get stream as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;Stream&gt;.</returns>
        public static async Task<Stream> GetStreamAsync(this HttpClient httpClient, string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {

            baseurl.IsNullThrow(nameof(baseurl));
            var url = URLHelper.CreateUrl(baseurl, resource, headers);

            var response = await SendAsync(httpClient,method, url, headers, content);
            EnsureSuccessCodeAsync(response);
            await response.Content.LoadIntoBufferAsync();
            var stream = await response.Content.ReadAsStreamAsync();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;

        }




        /// <summary>
        /// Execute get stream as an synchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url">The baseurl.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;Stream&gt;.</returns>
        public static Stream GetStream(this HttpClient httpClient, string url, Method method, HttpContent content = null)
        {
            return GetStream(httpClient,url, null, null, method, content);
        }


        /// <summary>
        /// Execute get stream as an synchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;Stream&gt;.</returns>
        public static Stream GetStream(this HttpClient httpClient, string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => GetStreamAsync(httpClient,baseurl, resource, headers, method, content));
        }

        #endregion

        #region GetBytes


        /// <summary>
        /// Execute get bytes as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url">The baseurl.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        public static async Task<byte[]> GetBytesAsync(this HttpClient httpClient, string url, Method method, HttpContent content = null)
        {
            return await GetBytesAsync(httpClient,url, null, null, method, content);
        }

        /// <summary>
        /// Execute get bytes as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        public static async Task<byte[]> GetBytesAsync(this HttpClient httpClient, string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {

            baseurl.IsNullThrow(nameof(baseurl));
            var url = URLHelper.CreateUrl(baseurl, resource, headers);

            var response = await SendAsync(httpClient,method, url, headers, content);
            EnsureSuccessCodeAsync(response);
            await response.Content.LoadIntoBufferAsync();
            var bytes = await response.Content.ReadAsByteArrayAsync();
            return bytes;

        }



        /// <summary>
        /// Execute get bytes as an synchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url">The baseurl.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        public static byte[] GetBytes(this HttpClient httpClient, string url, Method method, HttpContent content = null)
        {
            return GetBytes(httpClient,url, null, null, method, content);
        }


        /// <summary>
        /// Execute get bytes as an synchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        public static byte[] GetBytes(HttpClient httpClient,string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => GetBytesAsync(httpClient,baseurl, resource, headers, method, content));
        }

        #endregion

        #region Get


        /// <summary>
        /// execute get type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="deserializer"></param>
        /// <param name="url">The url.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public static async Task<T> GetAsync<T>(this HttpClient httpClient, Func<string, T> deserializer, string url, Method method, HttpContent content = null)
        {
            return await GetAsync(httpClient,deserializer, url, null, null, method, content);
        }

        /// <summary>
        /// execute get type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="deserializer"></param>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public static async Task<T> GetAsync<T>(this HttpClient httpClient,Func<string, T> deserializer, string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            baseurl.IsNullThrow(nameof(baseurl));
            var url = URLHelper.CreateUrl(baseurl, resource, headers);

            var response = await SendAsync(httpClient,method, url, headers, content);
            EnsureSuccessCodeAsync(response);
            var result = await response.Content.ReadAsStringAsync();
            return deserializer.Invoke(result);
        }


        /// <summary>
        /// execute get type as an synchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="deserializer"></param>
        /// <param name="url">The url.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public static T Get<T>(this HttpClient httpClient, Func<string, T> deserializer, string url, Method method, HttpContent content = null)
        {
            return Get(httpClient,deserializer, url, null, null, method, content);
        }

        /// <summary>
        /// execute get type as an synchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="deserializer"></param>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public static T Get<T>(this HttpClient httpClient, Func<string, T> deserializer, string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => GetAsync(httpClient,deserializer, baseurl, resource, headers, method, content));
        }


        #endregion



        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url">The URL.</param>
        /// <param name="fileStream"></param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static async Task<bool> DownloadFileAsync(this HttpClient httpClient, string url, FileStream fileStream)
        {
            using (var content = await GetHttpResponseAsync(httpClient,url, Method.Get))
            {
                if (content == null) return true;
                await content.Content.CopyToAsync(fileStream);
                return true;
            }
        }



        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url">The URL.</param>
        /// <param name="fileStream"></param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static void DownloadFile(this HttpClient httpClient,string url, FileStream fileStream)
        {
            AsyncHelper.RunSync(() => DownloadFileAsync(httpClient,url, fileStream));
        }


        /// <summary>
        /// Downloads file from url as an asynchronous operation. 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url">The URL.</param>
        /// <param name="fileStream">The filestream that content will be written to</param>
        /// <param name="progress">The progress.</param>
        /// <param name="buffer"></param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public static async Task DownloadFileAsync(HttpClient httpClient, string url, FileStream fileStream, IProgress<double> progress, byte[] buffer = null)
        {
            progress.Report(10);
            using (var response = await GetStreamAsync(httpClient,url, null, null, Method.Get))
            {
                if (response == null || response.Length <= 0)
                {
                    progress.Report(100);
                    return;
                }
                if (buffer == null) buffer = new byte[4 * 1024];
                int read;
                var max = response.Length;
                var currentprogress = 0;
                progress.Report(50);
                while ((read = response.Read(buffer, 0, buffer.Length)) > 0)
                {
                    currentprogress = currentprogress + read;
                    var realprogress = decimal.Divide(currentprogress, max) * 100;
                    if ((Convert.ToInt32(realprogress) / 2) > 0)
                        progress.Report((Convert.ToInt32(realprogress) / 2) + 50);
                    await fileStream.WriteAsync(buffer, 0, read, CancellationToken.None);
                }
                return;
            }
        }



        /// <summary>
        /// Downloads file from url as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url">The URL.</param>
        /// <param name="fileStream">The filestream that content will be written to</param>
        /// <param name="progress">The progress.</param>
        /// <param name="buffer"></param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public static void DownloadFile(this HttpClient httpClient, string url, FileStream fileStream, IProgress<double> progress, byte[] buffer = null)
        {
            AsyncHelper.RunSync(() => DownloadFileAsync(httpClient, url, fileStream, progress, buffer));
        }



        /// <summary>
        /// Ensures the success code asynchronous.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <exception cref="Exception"></exception>
        private static void EnsureSuccessCodeAsync(HttpResponseMessage response)
        {
            if (AlwaysEnsureSuccessCode)
                response.EnsureSuccessStatusCode();
        }




    }
}
