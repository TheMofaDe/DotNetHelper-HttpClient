
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using DotNetHelper_HttpClient.Enum;
using DotNetHelper_HttpClient.Extension;
using DotNetHelper_HttpClient.Helpers;
using DotNetHelper_HttpClient.Interface;
using DotNetHelper_HttpClient.Models;
using Polly;

namespace DotNetHelper_HttpClient.Services
{
    /// <summary>
    /// A Awesome Class For Restful Api Calls & And Downloading Files HttpRestfulClient.
    /// </summary>
    public class HttpRestfulClient : IRestfulClient
    {

       
        /// <summary>
        /// Gets or sets the handler.
        /// </summary>
        /// <value>The handler.</value>
        public HttpClientHandler Handler { get; set; } = new HttpClientHandler();
        /// <summary>
        /// Gets or sets a value indicating whether [reuse handler].
        /// </summary>
        /// <value><c>true</c> if [reuse handler]; otherwise, <c>false</c>.</value>
        private static bool ReuseHandler { get; } = false;


        /// <summary>
        /// Gets or sets the default timeout.
        /// </summary>
        /// <value>The default timeout.</value>
        public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(60);
        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>The encoding.</value>
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        /// <summary>
        /// Gets or sets a value indicating whether [always ensure success code].
        /// </summary>
        /// <value><c>true</c> if [always ensure success code]; otherwise, <c>false</c>.</value>
        public bool AlwaysEnsureSuccessCode { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public HttpClient Client { get; private set; }

        /// <summary>
        /// A func that will return a httpresponsemessage this method is used to integrate with polly
        /// </summary>
        public Func<Task<HttpResponseMessage>> HttpRequestExecuteAsync { get; set; }


        public HttpRestfulClient()
        {
            Client = new HttpClient(Handler, ReuseHandler) {Timeout = DefaultTimeout};
        }

        public HttpRestfulClient(Encoding encoding)
        {
            Encoding = encoding;
            Client = new HttpClient(Handler, ReuseHandler) {Timeout = DefaultTimeout};
        }


        public void ReInitialize(HttpClientHandler handle, bool reuseHandler)
        {
            Client = new HttpClient(handle, reuseHandler) {Timeout = DefaultTimeout};
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
               ,Method = method.MapToHttpMethod()

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
                return await Client.SendAsync(request);
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

        /// <summary>
        /// execute get response as an asynchronous operation.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> ExecuteGetResponseAsync(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            baseurl.IsNullThrow(nameof(baseurl));
            var url = URLHelper.CreateUrl(baseurl, resource, headers);

            var response = await SendAsync(method, url, headers, content);
            EnsureSuccessCodeAsync(response);
            var result = await response.Content.ReadAsStringAsync();
            return result;
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
        public string ExecuteGetResponse(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => ExecuteGetResponseAsync(baseurl, resource, headers, method, content));
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
        public async Task<HttpResponseMessage> ExecuteGetHttpResponseAsync(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            baseurl.IsNullThrow(nameof(baseurl));
            var url = URLHelper.CreateUrl(baseurl, resource, headers);

            var response = await SendAsync(method, url, headers, content);
            EnsureSuccessCodeAsync(response);
            return response;

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
        public HttpResponseMessage ExecuteGetHttpResponse(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => ExecuteGetHttpResponseAsync(baseurl, resource, headers, method, content));
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
        public async Task<Stream> ExecuteGetStreamAsync(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
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
        /// Execute get stream as an asynchronous operation.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;Stream&gt;.</returns>
        public Stream ExecuteGetStream(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => ExecuteGetStreamAsync(baseurl, resource, headers, method, content));
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
        public async Task<byte[]> ExecuteGetBytesAsync(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
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
        /// Execute get bytes as an asynchronous operation.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        public byte[] ExecuteGetBytes(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {
            return AsyncHelper.RunSync(() => ExecuteGetBytesAsync(baseurl, resource, headers, method, content));
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
        public async Task<T> ExecuteGetTypeAsync<T>(Func<string, T> deserializer, string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {

            baseurl.IsNullThrow(nameof(baseurl));
            var url = URLHelper.CreateUrl(baseurl, resource, headers);

            var response = await SendAsync(method, url, headers, content);
            EnsureSuccessCodeAsync(response);
            var result = await response.Content.ReadAsStringAsync();
            return deserializer.Invoke(result);
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
            public T ExecuteGetType<T>(Func<string, T> deserializer, string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
            {
             return AsyncHelper.RunSync(() => ExecuteGetTypeAsync(deserializer,baseurl,resource,headers,method,content));
            }




            /// <summary>
            /// Executes the get XML document.
            /// </summary>
            /// <param name="baseurl">The baseurl.</param>
            /// <param name="resource">The resource.</param>
            /// <param name="headers">The headers.</param>
            /// <param name="method">The method.</param>
            /// <param name="content">The content.</param>
            /// <returns>XmlDocument.</returns>
            public async Task<XmlDocument> ExecuteGetXmlDocumentAsync(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
            {

                baseurl.IsNullThrow(nameof(baseurl));
                var url = URLHelper.CreateUrl(baseurl, resource, headers);

                var response = await SendAsync(method, url, headers, content);
                EnsureSuccessCodeAsync(response);
                var  result = await response.Content.ReadAsStringAsync();

                var doc = new XmlDocument();
                doc.LoadXml(result);
                return doc;

            }


            /// <summary>
            /// Executes the get XML document.
            /// </summary>
            /// <param name="baseurl">The baseurl.</param>
            /// <param name="resource">The resource.</param>
            /// <param name="headers">The headers.</param>
            /// <param name="method">The method.</param>
            /// <param name="content">The content.</param>
            /// <returns>XmlDocument.</returns>
            public XmlDocument ExecuteGetXmlDocument(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
            {
                return AsyncHelper.RunSync(() => ExecuteGetXmlDocumentAsync(baseurl, resource, headers, method, content));
            }


            /// <summary>
            /// Downloads the file.
            /// </summary>
            /// <param name="url">The URL.</param>
            /// <param name="fileStream"></param>
            /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
            public async Task<bool> DownloadFileAsync(string url, FileStream fileStream)
        {
                using (var content = ExecuteGetHttpResponse(url, null, null, Method.Get))
                {
                    if (content == null) return true;
                    await content.Content.CopyToAsync(fileStream);
                    return true;
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
        public async Task<bool> DownloadFileAsync(string url, FileStream fileStream, IProgress<double> progress, byte[] buffer = null)
        {
            progress.Report(10);
            using (var response = await ExecuteGetStreamAsync(url, null, null, Method.Get))
            {
                if (response == null || response.Length <= 0)
                {
                    progress.Report(100);
                    return true;
                }
                if(buffer == null) buffer = new byte[4 * 1024];
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
                return true;
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
        public bool DownloadFile(string url, FileStream fileStream, IProgress<double> progress, byte[] buffer = null)
        {
            return AsyncHelper.RunSync(() => DownloadFileAsync(url, fileStream, progress, buffer));
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


