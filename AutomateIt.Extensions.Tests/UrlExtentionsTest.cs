using AutomateIt.Extensions.Extensions;
using Xunit;

namespace AutomateIt.Extensions.Tests
{
    public class UrlExtentionsTest
    {
        [Theory]
        [InlineData("http://moskva.dr-bee.ru/customers/products/?param1=value1", "/customers/products/?param1=value1")]
        [InlineData("http://moskva.dr-bee.ru/customers/products/?param1=value1#tag", "/customers/products/?param1=value1#tag")]
        public void CutBaseUrl(string s, string expected)
        {
            Assert.Equal(expected, s.CutBaseUrl());
        }
    }
}