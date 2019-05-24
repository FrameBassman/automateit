using AutomateIt.SmartSelector;
using Xunit;

namespace AutomateIt.Tests
{
	public class ScssBuilderTests
	{
		[Theory]
		[InlineData("div[~'text']", "//div[text()[contains(normalize-space(.),'text')]]")]
		[InlineData("div['text']", "//div[text()[normalize-space(.)='text']]")]
		[InlineData("div[src='1.png']['text']", "//div[@src='1.png'][text()[normalize-space(.)='text']]")]
		[InlineData("div[src=\"1.png\"]['text']", "//div[@src=\"1.png\"][text()[normalize-space(.)='text']]")]
		[InlineData(".classname#myid['text']", "//*[@id='myid'][contains(@class,'classname')][text()[normalize-space(.)='text']]")]
		[InlineData(".classname['mytext']", "//*[contains(@class,'classname')][text()[normalize-space(.)='mytext']]")]
		[InlineData("div.classname['mytext']", "//div[contains(@class,'classname')][text()[normalize-space(.)='mytext']]")]
		[InlineData(".classname1.classname2['mytext']",
			"//*[contains(@class,'classname1')][contains(@class,'classname2')][text()[normalize-space(.)='mytext']]")]
		[InlineData("div.classname1.classname2['mytext']",
			"//div[contains(@class,'classname1')][contains(@class,'classname2')][text()[normalize-space(.)='mytext']]")]
		[InlineData(".classname1['mytext'] .classname2['mytext']",
			"//*[contains(@class,'classname1')][text()[normalize-space(.)='mytext']]/descendant::*[contains(@class,'classname2')][text()[normalize-space(.)='mytext']]"
		)]
		[InlineData("div.classname1['mytext'] div.classname2['mytext']",
			"//div[contains(@class,'classname1')][text()[normalize-space(.)='mytext']]/descendant::div[contains(@class,'classname2')][text()[normalize-space(.)='mytext']]"
		)]
		[InlineData("#myid div['mytext']", "//*[@id='myid']/descendant::div[text()[normalize-space(.)='mytext']]")]
		[InlineData("div#myid div['mytext']", "//div[@id='myid']/descendant::div[text()[normalize-space(.)='mytext']]")]
		[InlineData("div#myid.classname div['mytext']",
			"//div[@id='myid'][contains(@class,'classname')]/descendant::div[text()[normalize-space(.)='mytext']]")]
		[InlineData("div#main-basket-info-div>ul>li['Тариф']>a", "//div[@id='main-basket-info-div']/ul/li[text()[normalize-space(.)='Тариф']]/a")]
		[InlineData("li[>h5>strong>a['mytext']]", "//li[h5/strong/a[text()[normalize-space(.)='mytext']]]")]
		[InlineData("li[>a]", "//li[a]")]
		[InlineData("li[>a[div]]", "//li[a[descendant::div]]")]
		[InlineData("tr[1]>td[last()]", "//tr[1]/td[last()]")]
		[InlineData("img[src~'111.png']", "//img[contains(@src,'111.png')]")]
		[InlineData("#showThemesPanel,.genre-filter['text']", "//*[@id='showThemesPanel']|//*[contains(@class,'genre-filter')][text()[normalize-space(.)='text']]")]
		[InlineData(">div.toggle-drop>ul>li>span['Вечером']", "//child::div[contains(@class,'toggle-drop')]/ul/li/span[text()[normalize-space(.)='Вечером']]")]
		[InlineData("li[10]>div.news-block", "//li[10]/div[contains(@class,'news-block')]")]
		[InlineData("td[h3>span['Категории, на которые вы уже подписаны']]>div>div", "//td[descendant::h3/span[text()[normalize-space(.)='Категории, на которые вы уже подписаны']]]/div/div")]
		//[InlineData("tr[span.ng-binding[descendant-or-self::*['{0}']]]", "tr[descendant::span[contains(@class,'ng-binding')][descendant-or-self::*[normalize-space(text())='{0}'])]]")]
		[InlineData("button[.km-icon.km-email-attachments]+ul", "//button[descendant::*[contains(@class,'km-icon')][contains(@class,'km-email-attachments')]]/following-sibling::ul")]
		[InlineData("[data-toggle='collapse'][1]", "//*[@data-toggle='collapse'][1]")]
		[InlineData("input[translate(@type, 'B', 'b')='button']", "input[translate(@type, 'B', 'b')='button']")]
		[InlineData("div>span[not(a)]", "//div/span[not(a)]")]
		[InlineData("div>span[position() mod 2 = 1 and position() > 1]", "//div/span[position() mod 2 = 1 and position() > 1]")]
		public void ConvertScssOnlyToXpath(string scssSelector, string result)
		{
			// .Arrange
			// .Act
			var scss = ScssBuilder.Create(scssSelector);
			// .Assert
			Assert.Equal(result, scss.Xpath);
			Assert.Null(scss.Css);
		}

		[Theory]
		[InlineData("span[data-bind='text: Title']", "//span[@data-bind='text: Title']")]
		[InlineData("#searchPreferences button[type='submit']", "//*[@id='searchPreferences']/descendant::button[@type='submit']")]
		[InlineData("label:contains('Law Firm')", "//label[text()[contains(normalize-space(.),'Law Firm')]]")]
		public void ConvertScssToXpath(string scssSelector, string result)
		{
			// .Arrange
			// .Act
			var scss = ScssBuilder.Create(scssSelector);
			// .Assert
			Assert.Equal(result, scss.Xpath);
			Assert.NotNull(scss.Css);
		}

		[Theory]
		[InlineData("#myid", "#myid")]
		[InlineData("div#myid", "div#myid")]
		[InlineData("div#myid.classname", "div#myid.classname")]
		[InlineData(".classname", ".classname")]
		[InlineData("div.classname", "div.classname")]
		[InlineData(".classname1.classname2", ".classname1.classname2")]
		[InlineData("div.classname1.classname2", "div.classname1.classname2")]
		[InlineData(".classname1 .classname2", ".classname1 .classname2")]
		[InlineData("div.classname1 div.classname2", "div.classname1 div.classname2")]
		[InlineData("div[src='1.png']", "div[src='1.png']")]
		[InlineData("div[src=\"1.png\"]", "div[src=\"1.png\"]")]
		[InlineData(">.search-bar", ">.search-bar")]
		[InlineData(".nav-section >.search-bar", ".nav-section >.search-bar")]
		[InlineData(".nav-section >.search-bar ul", ".nav-section >.search-bar ul")]
		[InlineData("#js-documentContentArea>div>p:nth-child(1)", "#js-documentContentArea>div>p:nth-child(1)")]
		[InlineData("#searchQueryInput,#km_id_search_form_search_hint", "#searchQueryInput,#km_id_search_form_search_hint")]
		[InlineData("label:contains('Law Firm')", "label:contains('Law Firm')")]
		[InlineData("span:nth-child(2n+1)", "span:nth-child(2n+1)")]
		public void ConvertScssToCss(string scssSelector, string result)
		{
			var scss = ScssBuilder.Create(scssSelector);
			Assert.Equal(result, scss.Css);
			Assert.NotNull(scss.Xpath);
		}

		[Theory]
		[InlineData("span:nth-child(2n+1)", "span:nth-child(2n+1)")]
		public void ConvertScssOnlyToCss(string scssSelector, string result)
		{
			var scss = ScssBuilder.Create(scssSelector);
			Assert.Equal(result, scss.Css);
			//            Assert.IsNull(scss.Xpath);
		}
	}
}