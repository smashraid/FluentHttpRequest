// <copyright file="RequestBuilderTest.cs" company="Rafael Lopez">Copyright ©  2017</copyright>
using System;
using FluentHttpRequest;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace FluentHttpRequest.Tests
{
    /// <summary>This class contains parameterized unit tests for RequestBuilder</summary>
    [PexClass(typeof(RequestBuilder))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class RequestBuilderTest
    {
        /// <summary>Test stub for Get(String)</summary>
        [PexMethod]
        public string GetTest([PexAssumeUnderTest]RequestBuilder target, string path)
        {
            string result = target.Get(path);
            return result;
            // TODO: add assertions to method RequestBuilderTest.GetTest(RequestBuilder, String)
        }
    }
}
