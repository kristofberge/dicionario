using System;
using NUnit.Framework;
using Moq;
using Moq.Protected;
using System.Net.Http;
using Dicionario.Exceptions;

namespace Dicionario.Tests
{
    [TestFixture]
    public class WordSearcherTests
    {
        private Mock<DicionarioClient> _dicionario;
        private TestableWordSearcher _wordSearcher;

        [TestFixtureSetUp]
        public void Setup()
        {
            _dicionario = new Mock<DicionarioClient>() { CallBase = true };
            string[] dictionaryValues = new string[] { "a", "b", "c", "d", "e" };
            _dicionario.Setup(d => d.GetWordAtPosition(It.IsAny<int>())).Returns((int position) => dictionaryValues[position]);

            _wordSearcher = new TestableWordSearcher();
            _wordSearcher.SetDicionario(_dicionario.Object);
        }

        [Test]
        [ExpectedException(typeof(DicionarioEmptyException))]
        public void GetPositionOfWord_ThrowsDicionarioEmptyExceptionWhenDicionarioIsEmpty()
        {
            _dicionario.Setup(d => d.GetWordAtPosition(0)).Throws(new NoWordAtPositionException());
            
            _wordSearcher.GetPositionOfWord("");
        }

        [TestCase("0")] // appears alfabetically before first element
        [TestCase("2")] // appears alfabetically between frist and last element
        [TestCase("Z")] // appear alfabetically after last element
        [ExpectedException(typeof(WordNotFoundException))]
        public void DoBinarySearchOnDicionario_ThrowsWordNotFoundException(string word)
        {
            _wordSearcher.GetPositionOfWord(word);
        }

        [TestCase("a", 0)]
        [TestCase("b", 1)]
        [TestCase("c", 2)]
        [TestCase("d", 3)]
        [TestCase("e", 4)]
        public void DoBinarySearchOnDicionario_ReturnsExpectedPosition(string word, int expectedPosition)
        {
            Assert.AreEqual(_wordSearcher.GetPositionOfWord(word), expectedPosition);
        }
        
        [TestCase(10, 8, 5)] // should return after 1st increment
        [TestCase(50, 8, 5)] // should hit max after several increments
        [TestCase(10, 1, 3)] // should return after hitting last elemt of array + 1 more increment
        [TestCase(10, 11, 2)] // should return immediately
        public void FindMax_ReturnsCorrectValue(int lastElementOfArray, int start, int increment)
        {
            _dicionario.Setup(d => d.GetWordAtPosition(It.Is<int>(i => i <= lastElementOfArray))).Returns("WORD");
            _dicionario.Setup(d => d.GetWordAtPosition(It.Is<int>(i => i > lastElementOfArray))).Throws(new NoWordAtPositionException());

            int max = _wordSearcher.GetBaseFindMaxResult(start, increment);

            // Assert that returned max is between last element and last elemtent + increment
            Assert.IsTrue(max > lastElementOfArray && max <= lastElementOfArray + increment);
        }
    }

    internal class TestableWordSearcher : WordSearcher
    {
        public void SetDicionario(DicionarioClient dicionario)
        {
            Dicionario = dicionario;
        }

        protected override int FindMax(int start = 50000, int increment = 50000)
        {
            return 4;
        }

        public int GetBaseFindMaxResult(int start, int increment)
        {
            return base.FindMax(start, increment);
        }
    }
}
