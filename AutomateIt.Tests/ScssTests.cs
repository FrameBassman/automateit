using AutomateIt.SmartSelector;
using Xunit;

namespace AutomateIt.Tests
{
	public class ScssTests
	{
		[Theory]
		[InlineData("div", "div", "//div/descendant::div", "div div")]
		[InlineData("div", ">div", "//div/child::div", "div>div")]
		public void Run(string scssSelector1, string scssSelector2, string resultXpath, string resultCss)
		{
			var scss1 = ScssBuilder.Create(scssSelector1);
			var scss2 = ScssBuilder.Create(scssSelector2);
			var resultScss = scss1.Concat(scss2);
			Assert.Equal(resultXpath, resultScss.Xpath);
			Assert.Equal(resultCss, resultScss.Css);
		}
	}
}
