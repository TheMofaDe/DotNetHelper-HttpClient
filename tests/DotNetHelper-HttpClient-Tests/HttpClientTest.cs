using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetHelper_HttpClient.Enum;
using DotNetHelper_HttpClient.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DotNetHelper_HttpClient_Tests
{
    [TestFixture]
    public class HttpClientTest
    {

        public JsonObject ExpectedValue { get; set; } = new JsonObject()
        {
            Completed = false,Id = 1
            ,UserId = 1,Title = "delectus aut autem"
        };

        [Test]
        public void A()
        {
            var client = new HttpRestfulClient(Encoding.UTF8);
            var json = client.ExecuteGetResponse("https://jsonplaceholder.typicode.com/todos/1", null, null, Method.Get);
            var jsonObject = JsonConvert.DeserializeObject<JsonObject>(json);
            Assert.AreEqual(ExpectedValue,jsonObject);
        }


        [Test]
        public void Test_BaseUrl_Is_Used()
        {
            var client = new HttpRestfulClient(Encoding.UTF8)
            {
                Client = {BaseAddress = new Uri("https://jsonplaceholder.typicode.com/")}
            };
            var jsonObject = client.ExecuteGetResponse("todos/1", null, null, Method.Get);
            Assert.AreEqual(ExpectedValue, jsonObject);
        }
    }
}
