using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZstdSeekable.Internal;

namespace ZstdSeekable.Tests
{
    [TestClass]
    public class XxHash64Tests
    {
        [DataTestMethod]
        [DataRow("", 0xEF46DB3751D8E999UL)]
        [DataRow("a", 0xD24EC4F1A98C6E5BUL)]
        [DataRow("123456789", 0x8CB841DB40E6AE83UL)]
        [DataRow("Hello, world!", 0xF58336A78B6F9476UL)]
        [DataRow("The quick brown fox jumps over the lazy dog", 0x0B242D361FDA71BCUL)]
        public void MatchesReferenceVectors(string input, ulong expected)
        {
            Assert.AreEqual(expected, XxHash64.Hash(Encoding.ASCII.GetBytes(input)));
        }
    }
}