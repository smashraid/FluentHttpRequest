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

        IFluentHttp request = new RequestBuilder();

        [TestMethod()]
        public void ExecuteTest()
        {

            //request.Create().AddParam().AddParam().AddBodyParam().AddHeader()
            //    .Get().Extract().Fill<>();

            List<MibResult> response = request
                .Create("https://lm.cignium.com/run/cignium/metlife/dev/mibjobconfiguration/")
                .Get().Fill<List<MibResult>>();

            var posts = request.Create(baseUrl + "posts")
                .Get().Fill<IEnumerable<Post>>();

            Assert.AreEqual(3, response.Count);
        }

        [TestMethod()]
        public void GetQueryStringTest()
        {
            var result = request.Create("https://lm.cignium.com/run/cignium/lga/dev/view-pdf-forms")
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

            var r = await request.Create("https://lm.cignium.com/run/cignium/lga/dev/view-pdf-forms")                
               .AddParam("Application Id", "164d7189-e40c-493c-8196-94b16bdd2c8a")
               .GetAsync();

            //r.Start();
            var res = r.Fill<JObject>();               
        }

        [TestMethod]
        public void RequestPost()
        {
          var response =  request
                .Create(baseUrl + "posts")
                .AddBodyParam("title", "foo")
                .AddBodyParam("body", "bar")
                .AddBodyParam("userId", "1")
                .Post();

            Assert.IsNotNull(response);
        }
    }
}