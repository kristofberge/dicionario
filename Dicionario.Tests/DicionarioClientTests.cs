using System;
using NUnit.Framework;
using Moq;
using Moq.Protected;
using System.Net.Http;
using Dicionario.Exceptions;

namespace Dicionario.Tests
{
    [TestFixture]
    public class DicionarioClientTests
    {
        private Mock<DicionarioClient> _dicionario;
        private HttpResponseMessage _response;
        private int _position;

        [TestFixtureSetUp]
        public void Setup()
        {
            _dicionario = new Mock<DicionarioClient>() { CallBase = true };
            _response = new HttpResponseMessage();
            _position = 0;

            _dicionario.Protected().Setup<HttpResponseMessage>("GetResponseFomDicionario", _position).Returns(_response);
        }

        [Test]
        [ExpectedException(typeof(NoWordAtPositionException))]
        public void GetWordAtPosition_ThrowsNoWordAtPositionExceptionWhenDicionarioReturns400()
        {
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            _dicionario.Object.GetWordAtPosition(_position);
        }

        [Test]
        public void GetWordAtPosition_ReturnsWordWhenDicionarioReturnsOK()
        {
            string word = "banana";
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.Content = new StringContent(word);

            string wordReturned = _dicionario.Object.GetWordAtPosition(_position);

            Assert.AreEqual(word, wordReturned);
        }
    }
}
