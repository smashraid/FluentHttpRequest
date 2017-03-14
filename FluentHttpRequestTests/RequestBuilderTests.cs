//using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentHttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using FluentHttpRequest.Helpers;
using FluentHttpRequest.CacheExtension;
using FluentHttpRequest.LifecycleManagement;
using FluentHttpRequest.FileExtension;
using NUnit.Framework;

namespace FluentHttpRequest.Tests
{
    [TestFixture]
    public class RequestBuilderTests
    {
        public class MibResult { public int ResultInfoCode { get; set; } public int From { get; set; } public int To { get; set; } public int RunEvery { get; set; } }

        public class Post { public int Id { get; set; } public int UserId { get; set; } public string Title { get; set; } public string Body { get; set; } }

        private const string baseUrl = "https://jsonplaceholder.typicode.com/";

        [Test]
        public void ExecuteTest()
        {
            var posts = RequestBuilder.Create(baseUrl + "posts")
                .Get().Fill<IEnumerable<Post>>();

            var post = RequestBuilder.Create(baseUrl + "posts/1")
                .Get().Fill<Post>();

            Assert.Greater(posts.Count(), 0);
            //FileBuilder.Ftp.Upload();
        }

        [Test]
        public void GetQueryStringTest()
        {
            var result = RequestBuilder.Create("https://lm.cignium.com/run/cignium/lga/dev/view-pdf-forms")
               .AddParam("Application Id", "164d7189-e40c-493c-8196-94b16bdd2c8a")
               .GetQueryString();

            Assert.AreEqual("https://lm.cignium.com/run/cignium/lga/dev/view-pdf-forms?Application+Id=164d7189-e40c-493c-8196-94b16bdd2c8a", result);
        }

        [Test]
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

        [Test]
        public async Task LyfecyclManagement_Test()
        {
            var process = await RequestBuilder
                .Project("metlife")
                .Env("dev")
                .Endpoint("mibjobconfiguration")
                .GetAsync();

            List<MibResult> mib = process.Fill<List<MibResult>>();

            //JObject app = RequestBuilder
            //    .Project("moo-tla")
            //    .Env("prod")
            //    .Endpoint("getfullapplication")
            //    .AddSecurityKey("key", "secret")
            //    .AddParam("Id", "0cd066f6-1eb3-4d14-8e5c-776c6b82782e")
            //    .Get()
            //    .Fill<JObject>();

            Assert.AreEqual(3, mib.Count);
        }

        [Test]
        public async Task RequestPost()
        {
            var response = await RequestBuilder
                  .Create(baseUrl + "posts")
                  .AddBodyParam("title", "foo")
                  .AddBodyParam("body", "bar")
                  .AddBodyParam("userId", "1")
                  .PostAsync();

            Assert.IsNotNull(response);
        }

        [Test]
        public void Cache_Test()
        {
            var posts = RequestBuilder.Create(baseUrl + "posts")
                  .Get().FillWithCache<IEnumerable<Post>>("Id", "Post", true);

            var post = RequestBuilder.Create(baseUrl + "posts/1")
                .Get().FillWithCache<Post>("Id", "Custom");
            //Cache.Storage.AddRange(posts,"Id", "Post");
            Cache.Storage.Add(post, post.Id, "Post");
            Post p = Cache.Storage.Get<Post>(1, "Post");
            p.Title = "This is an update of the cache test";
            Cache.Storage.Update(1, "Post", p);
            Post p2 = Cache.Storage.Get<Post>(1, "Post");
            Cache.Storage.Remove(1, "Post");
            var pList = Cache.Storage.GetAll<Post>("Post");
            Cache.Storage.RemoveAll("Post");
            Assert.AreEqual(posts.Count() - 1, pList.Count());
        }

        [Test]
        public async Task Save_File()
        {
            string sampple = "This is a test";
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}/test_save.txt";
            FileBuilder.Flat.WriteAsync(path, sampple);
            string result = await FileBuilder.Flat.ReadAsync(path);
            Assert.AreEqual(sampple, result);
        }
    }
}