namespace AutomateIt.SmartSelector
{
	using Extensions;
	using OpenQA.Selenium;

	public class Scss
	{
		public readonly string Css;
		public readonly string Xpath;
		public bool CombineWithRoot;

		public Scss(string xpath, string css, bool combineWithRoot = false)
		{
			Css = css;
			Xpath = xpath;
			CombineWithRoot = combineWithRoot;
		}

		public By By => string.IsNullOrEmpty(Css) ? By.XPath(Xpath) : By.CssSelector(Css);

		public string Value => string.IsNullOrEmpty(Css) ? Xpath : Css;

		public static string Concat(string scssSelector1, string scssSelector2)
		{
			return ScssBuilder.Concat(scssSelector1, scssSelector2).Value;
		}

		public Scss Concat(Scss scss2)
		{
			var resultXpath = XPathBuilder.Concat(Xpath, scss2?.Xpath);
			var resultCss = string.IsNullOrEmpty(Css) || string.IsNullOrEmpty(scss2.Css)
				? string.Empty
				: CssBuilder.Concat(Css, scss2.Css);
			return new Scss(resultXpath, resultCss);
		}

		public static By GetBy(string scssSelector1, string scssSelector2)
		{
			return ScssBuilder.Concat(scssSelector1, scssSelector2).By;
		}

		public static By GetBy(string scssSelector)
		{
			return ScssBuilder.CreateBy(scssSelector);
		}
	}
}