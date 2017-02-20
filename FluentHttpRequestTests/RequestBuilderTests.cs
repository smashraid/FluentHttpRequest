using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentHttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentHttpRequest.Tests
{
    [TestClass()]
    public class RequestBuilderTests
    {
        public class MibResult { public int ResultInfoCode { get; set; } public int From { get; set; } public int To { get; set; } public int RunEvery { get; set; } }

        [TestMethod()]
        public void CreateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteTest()
        {
            List<MibResult> response = (List<MibResult>) RequestBuilder.Create("https://lm.cignium.com/run/cignium/metlife/dev/mibjobconfiguration/").Execute().Extract("$").Fill<List<MibResult>>();

            Assert.AreEqual(3,response.Count);
        }
    }
}