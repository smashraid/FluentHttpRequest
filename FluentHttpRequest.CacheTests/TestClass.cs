using FluentHttpRequest.CacheExtension;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentHttpRequest.CacheTests
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            var rr = RequestBuilder.Create("").Get().Fill<int>();
            Cache.Storage.Add();
            Assert.Pass("Your first passing test");
        }
    }
}
