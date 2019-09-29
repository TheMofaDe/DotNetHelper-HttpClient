
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

        private bool IsFirstRequest { get; set; } 
        /// <inheritdoc />
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
        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the put and post only cancel token.
        /// </summary>
        /// <value>The put and post only cancel token.</value>
        public CancellationToken PutAndPostOnlyCancelToken { get; set; } = CancellationToken.None;
        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the default timeout.
        /// </summary>
        /// <value>The default timeout.</value>
        public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(60);
        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>The encoding.</value>
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        /// <inheritdoc />
        /// <summary>
        /// Gets or sets a value indicating whether [always ensure success code].
        /// </summary>
        /// <value><c>true</c> if [always ensure success code]; otherwise, <c>false</c>.</value>
        public bool AlwaysEnsureSuccessCode { get; set; } = true;


        public HttpClient Client { get; private set; }

        public Policy Policy { get; set; }  
 
   
        public HttpRestfulClient()
        {
            Client = new HttpClient(Handler,ReuseHandler);
            IsFirstRequest = true;
        }

        public HttpRestfulClient(Encoding encoding)
        {
            Encoding = encoding;
            Client = new HttpClient(Handler, ReuseHandler);
            IsFirstRequest = true;
        }


        public void ReInitialize(HttpClientHandler handle, bool reuseHandler)
        {
            Client = new HttpClient(handle , reuseHandler);
            IsFirstRequest = true;
        }





       









        /// <summary>
        /// do work as an asynchronous operation.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers"></param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        private async Task<HttpResponseMessage> DoWorkAsync(Method method, string url, List<Parameter> headers, HttpContent content = null)
        {


            if (IsFirstRequest)
            {
                Client.Timeout = DefaultTimeout;
                IsFirstRequest = false;
            }

            var request = new HttpRequestMessage(HttpMethod.Options, url)
            {
                Content = content
               ,Method = method.MapToHttpMethod()

            };
            if (!headers.IsNullOrEmpty())
            {
                headers.Where(p => p.Type == ParameterType.HttpHeader).ToList().ForEach(delegate(Parameter parameter)
                {
                    request.Headers.Add(parameter.Name,parameter.Value.ToString());
                });

                var cookies = headers.Where(p => p.Type == ParameterType.Cookie).Select(c => $"{c.Name}={c.Value}; ");
                if (cookies.Any())
                {
                    var value = string.Join(string.Empty, cookies);
                    value = value.ReplaceLastOccurrence("; ",string.Empty,StringComparison.OrdinalIgnoreCase);
                    request.Headers.Add("Cookie", value);
                }
           
            }

            if (Policy != null)
            {
                var pollyRequest = await Policy.ExecuteAsync(async () => await Client.SendAsync(request, PutAndPostOnlyCancelToken));
                return pollyRequest;
            }
            else
            {
                return await Client.SendAsync(request, PutAndPostOnlyCancelToken);
            }
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }



        /// <inheritdoc />
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

            var url = "";
            if (string.IsNullOrEmpty(baseurl))
            {
                return null;
            }
            else
            {
                url = URLHelper.CreateUrl(baseurl, resource, headers);
            }

            var response = await DoWorkAsync(method, url,headers,content);
            EnsureSuccessCodeAsync(response);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }


        /// <inheritdoc />
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

            var reason = $"";

            var url = "";
            if (string.IsNullOrEmpty(baseurl))
            {
                return null;
            }
            else
            {
                url = URLHelper.CreateUrl(baseurl, resource, headers);
            }
            var response = await DoWorkAsync(method, url,headers,content);
            EnsureSuccessCodeAsync(response);
            return response;

        }

        /// <inheritdoc />
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

            var url = "";
            if (string.IsNullOrEmpty(baseurl))
            {
                return null;
            }
            else
            {
                url = URLHelper.CreateUrl(baseurl, resource, headers);
            }

            var response = await DoWorkAsync(method, url,headers,content);
            EnsureSuccessCodeAsync(response);
            await response.Content.LoadIntoBufferAsync();
            var stream = await response.Content.ReadAsStreamAsync();
            stream.Position = 0;
            return stream;

        }


        /// <inheritdoc />
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

            var url = "";
            if (string.IsNullOrEmpty(baseurl))
            {
                return null;
            }
            else
            {
                url = URLHelper.CreateUrl(baseurl, resource, headers);
            }
            var response = await DoWorkAsync(method, url,headers,content);
            EnsureSuccessCodeAsync(response);
            await response.Content.LoadIntoBufferAsync();
            var bytes = await response.Content.ReadAsByteArrayAsync();
            return bytes;

        }

        /// <inheritdoc />
        /// <summary>
        /// execute get type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public async Task<T> ExecuteGetTypeAsync<T>(Func<string, T> deserializer, string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null) 
        {

            var url = "";
            if (string.IsNullOrEmpty(baseurl))
            {
                //  return null;
                return default;
            }
            else
            {
                url = URLHelper.CreateUrl(baseurl, resource, headers);
            }

            var response = await DoWorkAsync(method, url,headers,content);
            EnsureSuccessCodeAsync(response);
            var result = await response.Content.ReadAsStringAsync();
            var isJsonResponse = response.Content.Headers.ContentType.MediaType == "application/json";
           // if (isJsonResponse)
                // return JsonConvert.DeserializeObject<T>(result);
            return deserializer.Invoke(result);
        }




        ///// <inheritdoc />
        ///// <summary>
        ///// Downloads the file.
        ///// </summary>
        ///// <param name="url">The URL.</param>
        ///// <param name="filename">The filename.</param>
        ///// <param name="overwriteIfFileExist">if set to <c>true</c> [overwrite if file exist].</param>
        ///// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        //public bool DownloadFile(string url, string fullFilePath, FileOption option)
        //{
        //    var success = false;
        //    var file = new FileObject(fullFilePath);
        //    Task.Run(async () =>
        //    {
        //        using (var content = ExecuteGetHttpResponse(url, null, null, Method.Get))
        //        {
        //            if (content == null) return false;
        //            using (var fileStream = file.GetFileStream(option))
        //            {
        //                await content.Content.CopyToAsync(fileStream);
        //                success = true;
        //                return true;
        //            }
        //        }
        //    }).Wait();
        //    return success;
        //}


        ///// <inheritdoc />
        ///// <summary>
        ///// Downloads the file.
        ///// </summary>
        ///// <param name="url">The URL.</param>
        ///// <param name="fullFilePath"></param>
        ///// <param name="progress">The progress.</param>
        ///// <param name="option"></param>
        ///// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        //public bool DownloadFile(string url, string fullFilePath, IProgress<double> progress, FileOption option)
        //{

        //    progress.Report(10);
        //    var file = new FileObject(fullFilePath);
        //    progress.Report(30);
        //    using (var response = ExecuteGetStream(url, null, null, Method.Get))
        //    {
        //        progress.Report(50);
        //        if (response == null || response.Length <= 0)
        //        {
        //            progress.Report(100);
        //            return false;
        //        }
        //        var start = DateTime.Now;
        //        using (var fileStream = file.GetFileStream(option))
        //        {
        //            var buffer = new byte[4 * 1024];
        //            int read;
        //            var max = response.Length;
        //            var currentprogress = 0;
        //            while ((read = response.Read(buffer, 0, buffer.Length)) > 0)
        //            {
        //                currentprogress = currentprogress + read;
        //                var realprogress = decimal.Divide(currentprogress, max) * 100;
        //                if ((Convert.ToInt32(realprogress) / 2) > 0)
        //                    progress.Report((Convert.ToInt32(realprogress) / 2) + 50);
        //                fileStream.Write(buffer, 0, read);
        //            }
        //            var elapsedTimeInSeconds = DateTime.Now.Subtract(start).TotalSeconds;
        //            return true;
        //        }
        //    }
        //}


        ///// <inheritdoc />
        ///// <summary>
        ///// download file as an asynchronous operation.
        ///// </summary>
        ///// <param name="url">The URL.</param>
        ///// <param name="fullFilePath"></param>
        ///// <param name="progress">The progress.</param>
        ///// <param name="option"></param>
        ///// <returns>Task&lt;System.Boolean&gt;.</returns>
        //public async Task<bool> DownloadFileAsync(string url, string fullFilePath, IProgress<double> progress, FileOption option)
        //{
        //    progress.Report(10);
        //    var file = new FileObject(fullFilePath);
        //    progress.Report(30);
        //    using (var response = await ExecuteGetStreamAsync(url, null, null, Method.Get))
        //    {
        //        progress.Report(50);
        //        if (response == null || response.Length <= 0)
        //        {
        //            progress.Report(100);
        //            return false;
        //        }
        //        var start = DateTime.Now;
        //        using (var fileStream = file.GetFileStream(option))
        //        {
        //            var buffer = new byte[4 * 1024];

        //            int read;
        //            var max = response.Length;
        //            var currentprogress = 0;
        //            while ((read = response.Read(buffer, 0, buffer.Length)) > 0)
        //            {
        //                currentprogress = currentprogress + read;
        //                var realprogress = decimal.Divide(currentprogress, max) * 100;
        //                if ((Convert.ToInt32(realprogress) / 2) > 0)
        //                    progress.Report((Convert.ToInt32(realprogress) / 2) + 50);
        //                await fileStream.WriteAsync(buffer, 0, read, CancellationToken.None);
        //            }
        //            var elapsedTimeInSeconds = DateTime.Now.Subtract(start).TotalSeconds;
        //            return true;
        //        }
        //    }
        //}


        ///// <inheritdoc />
        ///// <summary>
        ///// download file as an asynchronous operation.
        ///// </summary>
        ///// <param name="url">The URL.</param>
        ///// <param name="fullFilePath"></param>
        ///// <param name="option"></param>
        ///// <returns>Task&lt;System.Boolean&gt;.</returns>
        //public async Task<bool> DownloadFileAsync(string url, string fullFilePath, FileOption option)
        //{
        //    var file = new FileObject(fullFilePath);

        //    using (var content = await ExecuteGetHttpResponseAsync(url, null, null, Method.Get))
        //    {
        //        if (content == null) return false;
        //        using (var fileStream = file.GetFileStream(option))
        //        {
        //            await content.Content.CopyToAsync(fileStream);
        //            return true;
        //        }

        //    }
        //}













        /// <inheritdoc />
        /// <summary>
        /// Executes the get response.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>System.String.</returns>
        public string ExecuteGetResponse(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {

            var url = "";
            if (string.IsNullOrEmpty(baseurl))
            {
                return null;
            }
            else
            {
                url = URLHelper.CreateUrl(baseurl, resource, headers);
            }
            string result = null;
            Task.Run(async () =>
            {
                //using (Client)
                //{
                    var response = await DoWorkAsync(method, url,headers,content);
                    EnsureSuccessCodeAsync(response);
                    result = await response.Content.ReadAsStringAsync();

               // }
            }, PutAndPostOnlyCancelToken).Wait(CancellationToken.None);
            return result;


        }


        /// <inheritdoc />
        /// <summary>
        /// Executes the get HTTP response.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>HttpResponseMessage.</returns>
        public HttpResponseMessage ExecuteGetHttpResponse(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {

            var url = "";
            if (string.IsNullOrEmpty(baseurl))
            {
                return null;
            }
            else
            {
                url = URLHelper.CreateUrl(baseurl, resource, headers);
            }
            HttpResponseMessage response = null;
            Task.Run(async () =>
            {
                //using (Client)
                //{
                    response = await DoWorkAsync(method, url,headers,content);
                    EnsureSuccessCodeAsync(response);
                //}
            }, PutAndPostOnlyCancelToken).Wait(PutAndPostOnlyCancelToken);
            return response;

        }

        /// <inheritdoc />
        /// <summary>
        /// Executes the get stream.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Stream.</returns>
        public Stream ExecuteGetStream(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {

            var url = "";
            if (string.IsNullOrEmpty(baseurl))
            {
                return null;
            }
            else
            {
                url = URLHelper.CreateUrl(baseurl, resource, headers);
            }
            Stream stream = null;
            Task.Run(async () =>
            {
                    var response = await DoWorkAsync(method, url,headers,content);
                    // Check that response was successful or throw exception
                    EnsureSuccessCodeAsync(response);
                    await response.Content.LoadIntoBufferAsync();
                    stream = await response.Content.ReadAsStreamAsync();
                    stream.Position = 0;
            }, PutAndPostOnlyCancelToken).Wait(PutAndPostOnlyCancelToken);
            return stream;
        }


        /// <inheritdoc />
        /// <summary>
        /// Executes the get bytes.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] ExecuteGetBytes(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null)
        {

            var url = "";
            if (string.IsNullOrEmpty(baseurl))
            {
                return null;
            }
            else
            {
                url = URLHelper.CreateUrl(baseurl, resource, headers);
            }
            byte[] bytes = null;
            Task.Run(async () =>
            {
               // using (Client)
               // {
                    var response = await DoWorkAsync(method, url,headers,content);
                    // Check that response was successful or throw exception
                    EnsureSuccessCodeAsync(response);
                    await response.Content.LoadIntoBufferAsync();
                    bytes = await response.Content.ReadAsByteArrayAsync();
              //  }
            }, PutAndPostOnlyCancelToken).Wait(CancellationToken.None);
            return bytes;

        }

        /// <inheritdoc />
        /// <summary>
        /// Executes the type of the get.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>T.</returns>
        public T ExecuteGetType<T>(Func<string, T> deserializer, string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null) 
        {


            var url = "";
            if (string.IsNullOrEmpty(baseurl))
            {
                // return null;
                return default;
            }
            else
            {
                url = URLHelper.CreateUrl(baseurl, resource, headers);
            }
            string result = null;
            var isJsonResponse = false;
            Task.Run(async () =>
            {
                var response = await DoWorkAsync(method, url,headers,content);
                    EnsureSuccessCodeAsync(response);
                    isJsonResponse = response.Content.Headers.ContentType.MediaType == "application/json";
                    result = await response.Content.ReadAsStringAsync();
            }, PutAndPostOnlyCancelToken).Wait(PutAndPostOnlyCancelToken);
            return deserializer.Invoke(result);


        }


        /// <inheritdoc />
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
            var url = "";
            if (string.IsNullOrEmpty(baseurl))
            {
                return null;
            }
            else
            {
                url = URLHelper.CreateUrl(baseurl, resource, headers);
            }
            string result = null;
            Task.Run(async () =>
            {
               // using (Client)
               // {
                    var response = await DoWorkAsync(method, url,headers,content);
                    EnsureSuccessCodeAsync(response);
                    result = await response.Content.ReadAsStringAsync();

              //  }
            }, PutAndPostOnlyCancelToken).Wait(PutAndPostOnlyCancelToken);
            var doc = new XmlDocument();
            doc.LoadXml(result);
            return doc;

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


