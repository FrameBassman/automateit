using AutomateIt.Framework.Page;
using AutomateIt.Framework.Service;
using Xunit;

namespace AutomateIt.Tests
{
    
    public class SelfMatchingRouterTests
    {
        [Fact]
        public void CompareTest()
        {
            // .Arrange
            var router = new SelfMatchingPagesRouter();

            // .Act
            router.RegisterPage<TestPage1>();
            router.RegisterPage<TestPage2>();

            // .Assert
            var sortedPages = router.GetSortedPages();

            Assert.Equal(typeof(TestPage2), sortedPages[0].GetType());
            Assert.Equal(typeof(TestPage1), sortedPages[1].GetType());
        }
    }

    public class TestPage1 : SelfMatchingPageBase
    {
        public override string AbsolutePath => "/somepath";

        public TestPage1()
        {
            Params["param1"] = "value1";
        }
    }
    public class TestPage2 : SelfMatchingPageBase
    {
        public override string AbsolutePath => "/somepath";
        public TestPage2()
        {
            Params["param1"] = "value1";
            Params["param2"] = "value2";
        }
    }
}