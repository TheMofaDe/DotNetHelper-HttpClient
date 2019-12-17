using System;
using System.IO;
using System.Text;
using DotNetHelper_HttpClient.Enum;
using DotNetHelper_HttpClient.Extension;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DotNetHelper.HttpClient.Tests
{
    [TestFixture]
    public class HttpClientShould
    {
        public System.Net.Http.HttpClient RestClient { get; } = new System.Net.Http.HttpClient();
        public JsonObject ExpectedValue { get; set; } = new JsonObject()
        {
            Completed = false,
            Id = 1,
            UserId = 1,
            Title = "delectus aut autem"
        };
        public string ExpectedJson { get; } = "{\n  \"userId\": 1,\n  \"id\": 1,\n  \"title\": \"delectus aut autem\",\n  \"completed\": false\n}";


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
        public void GetString_Return_Expected_String()
        {
            var str = RestClient.GetString($"https://jsonplaceholder.typicode.com/todos/1", Method.Get);
            Assert.That(str.Equals(ExpectedJson));
        }

        [Test]
        public void GetBytes_Return_Expected_Bytes()
        {
            var bytes = RestClient.GetBytes($"https://jsonplaceholder.typicode.com/todos/1", Method.Get);
            var expectedBytes = Encoding.Default.GetBytes(ExpectedJson);
            Assert.That(bytes.Compare(0, expectedBytes, 0, bytes.Length));
        }

        [Test]
        public void GetStream_Return_Expected_Stream()
        {
            var stream = RestClient.GetStream($"https://jsonplaceholder.typicode.com/todos/1", Method.Get);
            var expectedStream = ExpectedJson.ToStream();

            Assert.That(stream.Position == expectedStream.Position);
            Assert.That(stream.Length == expectedStream.Length);
            Assert.That(stream.ToString().Equals(expectedStream.ToString()));
        }



        [Test]
        public void Get_Return_Expected_Object()
        {
            var jsonObject = RestClient.Get(JsonConvert.DeserializeObject<JsonObject>, $"https://jsonplaceholder.typicode.com/todos/1", Method.Get);
            Assert.That(IsAMatch(ExpectedValue, jsonObject));
        }



        [Test]
        public void GetHttpResponse_Return_Success()
        {
            var httpResponse = RestClient.GetHttpResponse($"https://jsonplaceholder.typicode.com/todos/1", Method.Get);
            Assert.That(httpResponse.IsSuccessStatusCode);
            Assert.DoesNotThrow(() => httpResponse.EnsureSuccessStatusCode());
        }


        [Test]
        public void DownloadFile_Works()
        {
            var fileName = Path.Combine(Environment.CurrentDirectory, "TestFile.json");
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                RestClient.DownloadFile($"https://jsonplaceholder.typicode.com/todos/1", fileStream);
            }
            Assert.That(File.Exists(fileName));

            var fileContent = File.ReadAllText(fileName);
            File.Delete(fileName);
            Assert.That(fileContent.Equals(ExpectedJson));

        }


        //[Test]
        //public void Test_BaseUrl_Is_Used()
        //{
        //    RestClient.BaseAddress = new Uri(BaseUrl);
        //    var json = RestClient.GetString("todos/1", null, null, Method.Get);
        //    var jsonObject = JsonConvert.DeserializeObject<JsonObject>(json);
        //    Assert.IsTrue(IsAMatch(ExpectedValue, jsonObject));
        //}

        //[Test]
        //public void Test_BaseUrl_Is_()
        //{
        //    var client = new RestClient()
        //    {
        //        BaseAddress = new Uri(BaseUrl)
        //    };
        //    var json = client.GetString("todos/1", null, null, Method.Get);
        //    var jsonObject = JsonConvert.DeserializeObject<JsonObject>(json);
        //    Assert.IsTrue(IsAMatch(ExpectedValue, jsonObject));
        //}

        //[Test]
        //public void Test_BaseUrl_Is_Used_2()
        //{
        //    var client = new RestClient()
        //    {
        //        BaseAddress = new Uri("https://jsonplaceholder.typicode.com/")
        //    };
        //    var json = client.GetString("todos/1", Method.Get);
        //    var jsonObject = JsonConvert.DeserializeObject<JsonObject>(json);
        //    Assert.IsTrue(IsAMatch(ExpectedValue, jsonObject));
        //}
    }
}
