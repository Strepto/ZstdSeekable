using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZstdSeekable.Internal;

namespace ZstdSeekable.Tests
{
    [TestClass]
    public class ZstdSeekTableChecksumTests
    {
        [DataTestMethod]
        [DataRow("", 0x51D8E999U)]
        [DataRow("a", 0xA98C6E5BU)]
        [DataRow("123456789", 0x40E6AE83U)]
        [DataRow("Hello, world!", 0x8B6F9476U)]
        [DataRow("The quick brown fox jumps over the lazy dog", 0x1FDA71BCU)]
        public void MatchesReferenceVectors(string input, uint expected)
        {
            Assert.AreEqual(expected, ZstdSeekTableChecksum.Compute(Encoding.ASCII.GetBytes(input)));
        }
    }
}