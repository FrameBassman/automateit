using Xunit;

namespace AutomateIt.Extensions.Facts
{
    public class XpathBuilderFact
    {
        //*[@id='aaa1']/descendant::*[@id='bbb']|//*[@id='aaa2']/descendant::*[@id='bbb']
        [Theory]
        [InlineData("div", true)]
        [InlineData("//div", true)]
        [InlineData("//div[@id='myId']", true)]
        [InlineData("//div[text()='mytext']", true)]
        [InlineData("//div[text()='mytext' and @class='myclass']", true)]
        [InlineData("//div[@id='myId']/descendant::span", true)]
        [InlineData("//div[@id='myId1']|//div[@id='myId2']", true)]
        [InlineData("#myId", false)]
        [InlineData(".myclass", false)]
        public void IsXpath(string xpath, bool isXpath)
        {
            Assert.Equal(isXpath, XPathBuilder.IsXPath(xpath));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void RootIsEmpty(string root)
        {
            var relative = "div";
            Assert.Equal("//div", XPathBuilder.Concat(root, relative));
        }

        [Fact]
        public void ConcatAsDescendant()
        {
            var root = "//div";
            var relative = "*[@id='myid']";
            Assert.Equal("//div/descendant::*[@id='myid']", XPathBuilder.Concat(root, relative));
        }

        [Fact]
        public void InsertArgsToRelative()
        {
            var root = "//div";
            var relative = "*[@id='{0}']";
            Assert.Equal("//div/descendant::*[@id='myid']", XPathBuilder.Concat(root, relative, "myid"));

            root = "//div[@id='{0}']";
            relative = "*[@id='{0}']";
            Assert.Equal("//div[@id='{0}']/descendant::*[@id='myid']", XPathBuilder.Concat(root, relative, "myid"));
        }

        [Fact]
        public void LeaveAxis()
        {
            var root = "//div";
            var relative = "self::*[@id='myid']";
            Assert.Equal("//div/self::*[@id='myid']", XPathBuilder.Concat(root, relative));
        }

        [Fact]
        public void MakeRelative()
        {
            var root = "//*[@id='aaa1']";
            var relative = "//*[@id='bbb']";
            Assert.Equal("//*[@id='aaa1']/descendant::*[@id='bbb']", XPathBuilder.Concat(root, relative));
        }

        [Fact]
        public void MultipleRootXpath()
        {
            var root = "//*[@id='aaa1'] | //*[@id='aaa2']";
            var relative = "*[@id='bbb']";
            Assert.Equal(
                "//*[@id='aaa1']/descendant::*[@id='bbb']|//*[@id='aaa2']/descendant::*[@id='bbb']",
                XPathBuilder.Concat(root, relative, "myid"));
        }

        [Fact]
        public void RelativeIsEmpty()
        {
            var root = "//*[@id='aaa1']";
            var relative = "";
            Assert.Equal("//*[@id='aaa1']", XPathBuilder.Concat(root, relative));
        }
    }
}