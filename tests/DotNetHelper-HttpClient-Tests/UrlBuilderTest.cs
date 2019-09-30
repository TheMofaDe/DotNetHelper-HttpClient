using System;
using System.Collections.Generic;
using DotNetHelper_HttpClient.Enum;
using DotNetHelper_HttpClient.Helpers;
using DotNetHelper_HttpClient.Models;
using NUnit.Framework;

namespace DotNetHelper_HttpClient_Tests
{
    [TestFixture]
    public class UrlBuilderTests
    {
        [Test]
        public void Test_CreateUrl_WithNullResourceAndHeaders()
        {
            var url = "http://example.com/";
            var expected = new Uri(url);
            var output = new Uri(URLHelper.CreateUrl(url, null, null));
            Assert.AreEqual(expected, output);
        }


        [Test]
        public void Test_CreateUrl_With_BaseUrl_Resource_And_QueryStrings_Parameters()
        {
            var url = "http://example.com/resource?param1=value1&foo=bar,baz";
            var expected = new Uri(url);
            var parameters = new List<Parameter>()
            {
                new Parameter(ParameterType.QueryString){Name = "param1",Value = "value1",},
                new Parameter(ParameterType.QueryString){Name = "foo",Value = "bar,baz",}
            };
            var output = new Uri(URLHelper.CreateUrl("http://example.com", "resource", parameters));

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void Test_CreateUrl_With_BaseUrl_TrailingSlash_And_Resource_And_QueryStrings_Parameters()
        {
            var url = "http://example.com/resource?param1=value1&foo=bar,baz";
            var expected = new Uri(url);
            var parameters = new List<Parameter>()
            {
                new Parameter(ParameterType.QueryString){Name = "param1",Value = "value1",},
                new Parameter(ParameterType.QueryString){Name = "foo",Value = "bar,baz",}
            };
            var output = new Uri(URLHelper.CreateUrl("http://example.com/", "resource", parameters));

            Assert.AreEqual(expected, output);
        }


        [Test]
        public void Test_CreateUrl_With_BaseUrl_TrailingSlash_And_ResourceHavingAQueryString_With_QueryStrings_Parameters()
        {
            var url = "http://example.com/resource?test=test&param1=value1&foo=bar,baz";
            var expected = new Uri(url);
            var parameters = new List<Parameter>()
            {
                new Parameter(ParameterType.QueryString){Name = "param1",Value = "value1",},
                new Parameter(ParameterType.QueryString){Name = "foo",Value = "bar,baz",}
            };
            var output = new Uri(URLHelper.CreateUrl("http://example.com/", "resource?test=test", parameters));

            Assert.AreEqual(expected, output);
        }



        [Test]
        public void Test_CreateUrl_With_BaseUrl_NoSlash_And_ResourceHavingAQueryString_With_QueryStrings_Parameters()
        {
            var url = "http://example.com/resource?test=test&param1=value1&foo=bar,baz";
            var expected = new Uri(url);
            var parameters = new List<Parameter>()
            {
                new Parameter(ParameterType.QueryString){Name = "param1",Value = "value1",},
                new Parameter(ParameterType.QueryString){Name = "foo",Value = "bar,baz",}
            };
            var output = new Uri(URLHelper.CreateUrl("http://example.com", "resource?test=test", parameters));

            Assert.AreEqual(expected, output);
        }


        [Test]
        public void Test_CreateUrl_With_BaseUrl_NoSlash_And_ResourceHavingAQueryString()
        {
            var url = "http://example.com/resource?test=test";
            var expected = new Uri(url);

            var output = new Uri(URLHelper.CreateUrl("http://example.com", "resource?test=test", null));

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void Test_CreateUrl_With_UrlSegment()
        {
            var url = "https://jsonplaceholder.typicode.com/todos";


            // var output = new Uri(URLHelper.CreateUrl("https://jsonplaceholder.typicode.com", "todos", parameters));

            //    Assert.AreEqual(new Uri(url), output);
        }

    }
}
