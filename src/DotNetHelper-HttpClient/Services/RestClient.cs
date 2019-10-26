// NOTES :: https://visualstudiomagazine.com/blogs/tool-tracker/2019/09/using-http.aspx
//HttpClient is intended to be instantiated once and re-used throughout the life of an application.Especially in server applications,
//creating a new HttpClient instance for every request will exhaust the number of sockets available under heavy loads
//This will result in SocketException errors.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DotNetHelper_HttpClient.Enum;
using DotNetHelper_HttpClient.Extension;
using DotNetHelper_HttpClient.Helpers;
using DotNetHelper_HttpClient.Interface;
using DotNetHelper_HttpClient.Models;


namespace DotNetHelper_HttpClient.Services
{


    /// <summary>
    /// A Awesome Class For Restful Api Calls & And Downloading Files HttpRestfulClient.
    /// </summary>
    public class RestClient : HttpClient, IRestClient
    {

        /// <summary>
        /// Gets or sets a value indicating whether [always ensure success code].
        /// </summary>
        /// <value><c>true</c> if [always ensure success code]; otherwise, <c>false</c>.</value>
        public bool AlwaysEnsureSuccessCode { get; set; } = false;


        /// <summary>
        /// A func that will return a httpresponsemessage this method is used to integrate with polly
        /// </summary>
        public Func<Task<HttpResponseMessage>> HttpRequestExecuteAsync { get; set; }


        public RestClient(HttpMessageHandler httpClientHandler, bool disposeHandler) : base(httpClientHandler, disposeHandler)
        {

        }

        public RestClient(HttpMessageHandler httpClientHandler) : base(httpClientHandler)
        {

        }

        public RestClient()
        {

        }


        /// <summary>
        /// Sends a HttpRequest and returns a reponse as an asynchronous operation.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers"></param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        private async Task<HttpResponseMessage> SendAsync(Method method, string url, List<Parameter> headers, HttpContent content = null)
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

            if (HttpRequestExecuteAsync != null)
            {
                return await HttpRequestExecuteAsync.Invoke();
            }
            else
            {
                return await SendAsync(request);
            }
        }


        /// <summary>
        /// do work as an asynchronous operation.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers"></param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        private HttpResponseMessage Send(Method method, string url, List<Parameter> headers, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => SendAsync(method, url, headers, content));
        }


        #region GetString


        /// <summary>
        /// execute get response as an asynchronous operation.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> GetStringAsync(string url, Method method, HttpContent content = null)
        {
            return await GetStringAsync(url, null, null, Method.Get, null);
        }

        /// <summary>
        /// execute get response as an asynchronous operation.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> GetStringAsync(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            baseurl.IsNullThrow(nameof(baseurl));
            var url = URLHelper.CreateUrl(baseurl, resource, headers);

            var response = await SendAsync(method, url, headers, content);
            EnsureSuccessCodeAsync(response);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }


        /// <summary>
        /// execute get response as an synchronous operation.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public string GetString(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => GetStringAsync(baseurl, resource, headers, method, content));
        }

        /// <summary>
        /// execute get response as an synchronous operation.
        /// </summary>
        /// <param name="url">The baseurl.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public string GetString(string url, Method method, HttpContent content = null)
        {
            return GetString(url, null, null, method, content);
        }



        #endregion

        #region GetHttpResponse

        /// <summary>
        /// Execute get HTTP response as an asynchronous operation.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        public async Task<HttpResponseMessage> GetHttpResponseAsync(string url, Method method, HttpContent content = null)
        {
            return await GetHttpResponseAsync(url, null, null, method, content);
        }

        /// <summary>
        /// Execute get HTTP response as an asynchronous operation.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        public async Task<HttpResponseMessage> GetHttpResponseAsync(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            baseurl.IsNullThrow(nameof(baseurl));
            var url = URLHelper.CreateUrl(baseurl, resource, headers);

            var response = await SendAsync(method, url, headers, content);
            EnsureSuccessCodeAsync(response);
            return response;
        }



        /// <summary>
        /// Execute get HTTP response as an synchronous operation.
        /// </summary>
        /// <param name="url">The baseurl.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        public HttpResponseMessage GetHttpResponse(string url, Method method, HttpContent content = null)
        {
            return GetHttpResponse(url, null, null, method, content);
        }

        /// <summary>
        /// Execute get HTTP response as an synchronous operation.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        public HttpResponseMessage GetHttpResponse(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => GetHttpResponseAsync(baseurl, resource, headers, method, content));
        }


        #endregion

        #region GetStream


        /// <summary>
        /// Execute get stream as an asynchronous operation.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;Stream&gt;.</returns>
        public async Task<Stream> GetStreamAsync(string url, Method method, HttpContent content = null)
        {
            return await GetStreamAsync(url, null, null, method, content);
        }


        /// <summary>
        /// Execute get stream as an asynchronous operation.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;Stream&gt;.</returns>
        public async Task<Stream> GetStreamAsync(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {

            baseurl.IsNullThrow(nameof(baseurl));
            var url = URLHelper.CreateUrl(baseurl, resource, headers);

            var response = await SendAsync(method, url, headers, content);
            EnsureSuccessCodeAsync(response);
            await response.Content.LoadIntoBufferAsync();
            var stream = await response.Content.ReadAsStreamAsync();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;

        }




        /// <summary>
        /// Execute get stream as an synchronous operation.
        /// </summary>
        /// <param name="url">The baseurl.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;Stream&gt;.</returns>
        public Stream GetStream(string url, Method method, HttpContent content = null)
        {
            return GetStream(url, null, null, method, content);
        }


        /// <summary>
        /// Execute get stream as an synchronous operation.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;Stream&gt;.</returns>
        public Stream GetStream(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => GetStreamAsync(baseurl, resource, headers, method, content));
        }

        #endregion

        #region GetBytes


        /// <summary>
        /// Execute get bytes as an asynchronous operation.
        /// </summary>
        /// <param name="url">The baseurl.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        public async Task<byte[]> GetBytesAsync(string url, Method method, HttpContent content = null)
        {
            return await GetBytesAsync(url, null, null, method, content);
        }

        /// <summary>
        /// Execute get bytes as an asynchronous operation.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        public async Task<byte[]> GetBytesAsync(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {

            baseurl.IsNullThrow(nameof(baseurl));
            var url = URLHelper.CreateUrl(baseurl, resource, headers);

            var response = await SendAsync(method, url, headers, content);
            EnsureSuccessCodeAsync(response);
            await response.Content.LoadIntoBufferAsync();
            var bytes = await response.Content.ReadAsByteArrayAsync();
            return bytes;

        }



        /// <summary>
        /// Execute get bytes as an synchronous operation.
        /// </summary>
        /// <param name="url">The baseurl.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        public byte[] GetBytes(string url, Method method, HttpContent content = null)
        {
            return GetBytes(url, null, null, method, content);
        }


        /// <summary>
        /// Execute get bytes as an synchronous operation.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        public byte[] GetBytes(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => GetBytesAsync(baseurl, resource, headers, method, content));
        }

        #endregion

        #region Get


        /// <summary>
        /// execute get type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="url">The url.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public async Task<T> GetAsync<T>(Func<string, T> deserializer, string url, Method method, HttpContent content = null)
        {
            return await GetAsync(deserializer, url, null, null, method, content);
        }

        /// <summary>
        /// execute get type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public async Task<T> GetAsync<T>(Func<string, T> deserializer, string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            baseurl.IsNullThrow(nameof(baseurl));
            var url = URLHelper.CreateUrl(baseurl, resource, headers);

            var response = await SendAsync(method, url, headers, content);
            EnsureSuccessCodeAsync(response);
            var result = await response.Content.ReadAsStringAsync();
            return deserializer.Invoke(result);
        }


        /// <summary>
        /// execute get type as an synchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="url">The url.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public T Get<T>(Func<string, T> deserializer, string url, Method method, HttpContent content = null)
        {
            return Get<T>(deserializer, url, null, null, method, content);
        }

        /// <summary>
        /// execute get type as an synchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public T Get<T>(Func<string, T> deserializer, string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => GetAsync(deserializer, baseurl, resource, headers, method, content));
        }


        #endregion



        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="fileStream"></param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public async Task<bool> DownloadFileAsync(string url, FileStream fileStream)
        {
            using (var content = await GetHttpResponseAsync(url, Method.Get))
            {
                if (content == null) return true;
                await content.Content.CopyToAsync(fileStream);
                return true;
            }
        }



        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="fileStream"></param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public void DownloadFile(string url, FileStream fileStream)
        {
            AsyncHelper.RunSync(() => DownloadFileAsync(url, fileStream));
        }


        /// <summary>
        /// Downloads file from url as an asynchronous operation. 
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="fileStream">The filestream that content will be written to</param>
        /// <param name="progress">The progress.</param>
        /// <param name="buffer"></param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task DownloadFileAsync(string url, FileStream fileStream, IProgress<double> progress, byte[] buffer = null)
        {
            progress.Report(10);
            using (var response = await GetStreamAsync(url, null, null, Method.Get))
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
        /// <param name="url">The URL.</param>
        /// <param name="fileStream">The filestream that content will be written to</param>
        /// <param name="progress">The progress.</param>
        /// <param name="buffer"></param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public void DownloadFile(string url, FileStream fileStream, IProgress<double> progress, byte[] buffer = null)
        {
            AsyncHelper.RunSync(() => DownloadFileAsync(url, fileStream, progress, buffer));
        }


        /// <summary>
        /// Ensures the success code asynchronous.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <exception cref="Exception"></exception>
        private void EnsureSuccessCodeAsync(HttpResponseMessage response)
        {
            if (AlwaysEnsureSuccessCode)
                response.EnsureSuccessStatusCode();
        }

    }







}


