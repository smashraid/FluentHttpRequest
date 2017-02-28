using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentHttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using FluentHttpRequest.Helpers;

namespace FluentHttpRequest.Tests
{
    [TestClass()]
    public class RequestBuilderTests
    {
        public class MibResult { public int ResultInfoCode { get; set; } public int From { get; set; } public int To { get; set; } public int RunEvery { get; set; } }

        public class Post { public int Id { get; set; } public int UserId { get; set; } public string Title { get; set; } public string Body { get; set; } }

        private const string baseUrl = "https://jsonplaceholder.typicode.com/";

        [TestMethod()]
        public void ExecuteTest()
        {
            List<MibResult> response = (List<MibResult>)RequestBuilder
                .Create("https://lm.cignium.com/run/cignium/metlife/dev/mibjobconfiguration/")
                .Get().Fill<List<MibResult>>();

            List<Post> posts = (List<Post>) RequestBuilder.Create(baseUrl + "posts")
                .Get().Fill<List<Post>>();

            Assert.AreEqual(3, response.Count);
        }

        [TestMethod()]
        public void GetQueryStringTest()
        {
            var request = RequestBuilder.Create("https://lm.cignium.com/run/cignium/lga/dev/view-pdf-forms")
               .AddParam("Application Id", "164d7189-e40c-493c-8196-94b16bdd2c8a")
               .GetQueryString();
            
            Assert.AreEqual("https://lm.cignium.com/run/cignium/lga/dev/view-pdf-forms?Application+Id=164d7189-e40c-493c-8196-94b16bdd2c8a", request);
        }

        [TestMethod()]
        public async Task RequestLMTest()
        {
            //var request = RequestBuilder.Create("https://lm.cignium.com/run/cignium/lga/dev/view-pdf-forms")
            //   .AddParam("Application Id", "164d7189-e40c-493c-8196-94b16bdd2c8a")
            //   .Execute()
            //   .Fill<JObject>();

            var r = await RequestBuilder.Create("https://lm.cignium.com/run/cignium/lga/dev/view-pdf-forms")                
               .AddParam("Application Id", "164d7189-e40c-493c-8196-94b16bdd2c8a")
               .GetAsync();

            //r.Start();
            var res = r.Fill<JObject>();               
        }

        [TestMethod]
        public void RequestPost()
        {
          var response =  RequestBuilder
                .Create(baseUrl + "posts")
                .AddBodyParam("title", "foo")
                .AddBodyParam("body", "bar")
                .AddBodyParam("userId", "1")
                .Post();

            Assert.IsNotNull(response);
        }
    }
}