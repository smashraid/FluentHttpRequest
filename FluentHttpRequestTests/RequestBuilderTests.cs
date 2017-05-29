//using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentHttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using FluentHttpRequest.CacheExtension;
using FluentHttpRequest.LifecycleManagement;
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
        public void Request_Get()
        {
            var posts = RequestBuilder.Create(baseUrl + "posts")
                .Get<IEnumerable<Post>>();

            var post = RequestBuilder.Create(baseUrl + "posts/1")
                .Get<Post>();

            var post2 = RequestBuilder.Create(baseUrl + "posts/2")
                .Get<string>();

            Assert.NotNull(post);
            Assert.Greater(posts.Count(), 0);
            //FileBuilder.Ftp.Upload();
        }

        [Test]
        public async Task Request_Get_Async()
        {
            var posts = await RequestBuilder.Create(baseUrl + "posts")
                .GetAsync<IEnumerable<Post>>();

            var post = await RequestBuilder.Create(baseUrl + "posts/1")
                .GetAsync<Post>();

            Assert.NotNull(post);
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
        public async Task Request_LyfecyclManagement_Get_Async()
        {
            var result = await RequestBuilder
                .Project("metlife")
                .Env("dev")
                .Endpoint("mibjobconfiguration")
                .GetAsync<List<MibResult>>();



            //JObject app = RequestBuilder
            //    .Project("moo-tla")
            //    .Env("prod")
            //    .Endpoint("getfullapplication")
            //    .AddSecurityKey("key", "secret")
            //    .AddParam("Id", "0cd066f6-1eb3-4d14-8e5c-776c6b82782e")
            //    .Get()
            //    .Fill<JObject>();

            Assert.AreEqual(4, result.Count);
        }

        [Test]
        public void Request_Post()
        {
            var response = RequestBuilder
                  .Create(baseUrl + "posts")
                  .AddBodyParam("title", "foo")
                  .AddBodyParam("body", "bar")
                  .AddBodyParam("userId", "1")
                  .Post<string>();

            Assert.IsNotNull(response);
        }

        [Test]
        public async Task Request_Post_Async()
        {
            var response = await RequestBuilder
                  .Create(baseUrl + "posts")
                  .AddBodyParam("title", "foo")
                  .AddBodyParam("body", "bar")
                  .AddBodyParam("userId", "1")
                  .PostAsync<string>();

            Assert.IsNotNull(response);
        }

        [Test]
        public void Request_Get_WithCache()
        {
            var posts = RequestBuilder.Create(baseUrl + "posts")
                  .Get<IEnumerable<Post>>().WithCache("Id").AndFallback();

            var post = RequestBuilder.Create(baseUrl + "posts/1")
                .Get<Post>();

            Cache.Storage.AddRange(posts,"Id", "Post");
            Cache.Storage.Add(post, post.Id, "Post");
            Post p = Cache.Storage.Get<Post>(1, "Post");
            p.Title = "This is an update of the cache test";
            Cache.Storage.Update(1, "Post", p);
            Post p2 = Cache.Storage.Get<Post>(1, "Post");
            Cache.Storage.Remove(1, "Post");
            var pList = Cache.Storage.GetAll<Post>("Post");
            //Cache.Storage.RemoveAll("Post");
            Assert.AreEqual(posts.Count() - 1, pList.Count());
        }

        [Test]
        public async Task Save_File()
        {
            Utils utils = new Utils();
            string sample = "This is a test";
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}/test_save1.txt";
            utils.WriteAsync(path, sample);
            string result = await utils.ReadAsync(path);
            Assert.AreEqual(sample, result);
        }
    }
}