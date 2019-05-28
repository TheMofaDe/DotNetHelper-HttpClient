// ***********************************************************************
// Assembly         : TheMoFaDe:
// Author           : Joseph McNeal Jr 
// Created          : 02-10-2017
//
// Last Modified By : Joseph McNeal Jr
// Last Modified On : 09-04-2017
// ***********************************************************************
// <copyright>
//             Copyright (C) 2017 Joseph McNeal Jr - All Rights Reserved
//             You may use, distribute and modify this code under the
//             terms of the TheMoFaDe: license,
//             You should have received a copy of the TheMoFaDe: license with
//             this file. If not, please write to: josephmcnealjr@mofade.com
//</copyright>
// <summary>   </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using DotNetHelper_Contracts.Enum;
using DotNetHelper_HttpClient.Models;
using Polly;

namespace DotNetHelper_HttpClient.Interface
{
    public interface IRestfulClient
    {

 

  
   



        /// <summary>
        /// execute get response as an asynchronous operation.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        Task<string> ExecuteGetResponseAsync(string baseurl, string resource, List<Parameter> headers, Method method, HttpContent content = null);


        /// <summary>
        /// Execute get HTTP response as an asynchronous operation.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        Task<HttpResponseMessage> ExecuteGetHttpResponseAsync(string baseurl, string resource, List<Parameter> headers,
            Method method, HttpContent content = null);

        /// <summary>
        /// Execute get stream as an asynchronous operation.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;Stream&gt;.</returns>
        Task<Stream> ExecuteGetStreamAsync(string baseurl, string resource, List<Parameter> headers, Method method,
            HttpContent content = null);


        /// <summary>
        /// Execute get bytes as an asynchronous operation.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        Task<byte[]> ExecuteGetBytesAsync(string baseurl, string resource, List<Parameter> headers, Method method,
            HttpContent content = null);

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
        Task<T> ExecuteGetTypeAsync<T>(string baseurl, string resource, List<Parameter> headers, Method method,
            HttpContent content = null); // where T : class; 


        /// <summary>
        /// Executes the get response.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>System.String.</returns>
        string ExecuteGetResponse(string baseurl, string resource, List<Parameter> headers, Method method,
            HttpContent content = null);



        /// <summary>
        /// Executes the get HTTP response.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>HttpResponseMessage.</returns>
        HttpResponseMessage ExecuteGetHttpResponse(string baseurl, string resource, List<Parameter> headers,
            Method method, HttpContent content = null);


        /// <summary>
        /// Executes the get stream.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>Stream.</returns>
        Stream ExecuteGetStream(string baseurl, string resource, List<Parameter> headers, Method method,
            HttpContent content = null);



        /// <summary>
        /// Executes the get bytes.
        /// </summary>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>System.Byte[].</returns>
        byte[] ExecuteGetBytes(string baseurl, string resource, List<Parameter> headers, Method method,
            HttpContent content = null);


        /// <summary>
        /// Executes the type of the get.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseurl">The baseurl.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>T.</returns>
        T ExecuteGetType<T>(string baseurl, string resource, List<Parameter> headers, Method method,
            HttpContent content = null);




    }
}
