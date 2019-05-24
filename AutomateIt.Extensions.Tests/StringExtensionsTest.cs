using AutomateIt.Extensions.Extensions;
using Xunit;

namespace AutomateIt.Extensions.Tests
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData("-5", "-5")]
        [InlineData("text -5 text", "-5")]
        [InlineData("text 1.2 text", "1.2")]
        [InlineData("text 1,2 text", "1,2")]
        public void FindNumber(string text, string expected)
        {
            Assert.Equal(expected, text.FindNumber());
        }

        [Theory]
        [InlineData("-5", -5)]
        public void AsDecimal(string text, decimal expected)
        {
            Assert.Equal(expected, text.AsDecimal());
        }

        [Theory]
        [InlineData("1", "1")]
        [InlineData("text 1 text", "1")]
        [InlineData("text 1.2 text", "1")]
        [InlineData("-1", "-1")]
        [InlineData("text -1 text", "-1")]
        [InlineData("text -1.2 text", "-1")]
        public void FindInt(string text, string expected)
        {
            Assert.Equal(expected, text.FindInt());
        }
    }
}