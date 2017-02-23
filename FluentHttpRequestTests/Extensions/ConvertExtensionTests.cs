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
    public class ConvertExtensionTests
    {
        class User { public string Name { get; set; } public string LastName { get; set; } }

        [TestMethod()]
        public void ToNameCollectionTest()
        {
            var user = new User() { Name = "Saulo", LastName = "Sabiv" };
            var nameValue = user.ToNameCollection();
            Assert.AreEqual(nameValue.Keys.Count, 2);
            Assert.AreEqual("Saulo", nameValue["Name"]);
            Assert.AreEqual("Sabiv", nameValue["LastName"]);
        }
    }
}