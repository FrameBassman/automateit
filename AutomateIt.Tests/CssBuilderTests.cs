using System;
using AutomateIt.SmartSelector;
using Xunit;

namespace AutomateIt.Tests
{
	public class CssBuilderTests
	{
		[Theory]
		[InlineData("div", "a", "div a")]
		[InlineData("div", ">a", "div>a")]
		public void ConcatTests(string root, string relative, string expectedResult)
		{
			// .Act
			var actualResult = CssBuilder.Concat(root, relative);
			// .Assert
			Assert.Equal(expectedResult, actualResult, StringComparer.Ordinal);
		}
	}
}
