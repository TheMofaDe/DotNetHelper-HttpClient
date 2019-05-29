using DotNetHelper_Contracts.Enum;
using DotNetHelper_HttpClient.Helpers;
using DotNetHelper_HttpClient.Services;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        //[Test]
        //public void Test12()
        //{
        //    var url = "https://binaryjazz.us/";
        //    var endpoint = "wp-json/genrenator/v1/genre/";
        //    var client = new HttpRestfulClient();

        //    var responseAsString = client.ExecuteGetResponse(url, endpoint, null, Method.Get);
        //}
    }
}