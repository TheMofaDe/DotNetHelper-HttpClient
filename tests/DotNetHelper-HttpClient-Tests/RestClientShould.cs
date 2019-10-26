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
    public class RestClientShould
    {
        public string BaseUrl { get; } = $"https://jsonplaceholder.typicode.com/";
        public JsonObject ExpectedValue { get; set; } = new JsonObject()
        {
            Completed = false,
            Id = 1,
            UserId = 1,
            Title = "delectus aut autem"
        };


        private bool IsAMatch(JsonObject one, JsonObject two)
        {
            return
            (
                one.Completed == two.Completed
                && one.Id == two.Id
                && one.Title == two.Title
                && one.UserId == two.UserId
            );
        }

        [Test]
        public void Test_GetReponse_AsString()
        {
            var client = new RestClient(Encoding.UTF8);
            var json = client.GetString("https://jsonplaceholder.typicode.com/todos/1",  Method.Get);
            var jsonObject = JsonConvert.DeserializeObject<JsonObject>(json);
            Assert.IsTrue(IsAMatch(ExpectedValue, jsonObject));
        }



        [Test]
        public void Test_BaseUrl_Is_Used()
        {
            var client = new RestClient(Encoding.UTF8)
            {
                Client = { BaseAddress = new Uri("https://jsonplaceholder.typicode.com/") }
            };
            var json = client.GetString("todos/1", null, null, Method.Get);
            var jsonObject = JsonConvert.DeserializeObject<JsonObject>(json);
            Assert.IsTrue(IsAMatch(ExpectedValue, jsonObject));
        }
    }
}
