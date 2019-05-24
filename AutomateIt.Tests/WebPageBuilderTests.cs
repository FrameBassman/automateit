using System;
using AutomateIt.Framework.Page;
using AutomateIt.Framework.PageElements;
using Xunit;

namespace AutomateIt.Tests
{
	
	public class WebPageBuilderTest
	{
		[Fact]
		public void DoNotAddRootWithouPrefix()
		{
			var page = new Page();
			var container = new Container(page, "//*[@id='rootelementid']");
			WebPageBuilder.InitComponents(page, container);
			Assert.Equal("//div[text()='mytext']", container.Component2.Xpath, StringComparer.Ordinal);
		}

		[Fact]
		public void ReplacePrefixWithRootSelector()
		{
			var page = new Page();
			var container = new Container(page, "//*[@id='rootelementid']");
			WebPageBuilder.InitComponents(page, container);
			Assert.Equal("//*[@id='rootelementid']/descendant::div[text()='mytext']", container.Component1.Xpath, StringComparer.Ordinal);
		}

		private class Container : ContainerBase
		{
			[WebComponent("root:div[text()='mytext']")]
			public Component Component1;
			[WebComponent("//div[text()='mytext']")]
			public Component Component2;


			public Container(IPage parent, string rootScss)
				: base(parent, rootScss)
			{
			}

			public override bool IsVisible()
			{
				throw new NotImplementedException();
			}

			public override bool IsNotVisible()
			{
				throw new NotImplementedException();
			}

			public override bool HasClass(string className)
			{
				throw new NotImplementedException();
			}

			public override bool IsDisabled()
			{
				throw new NotImplementedException();
			}

			public override T GetValue<T>()
			{
				throw new NotImplementedException();
			}

			public override void Click(int sleepTimeout = 0)
			{
				throw new NotImplementedException();
			}
		}

		private class Component : ComponentBase
		{
			public readonly string Xpath;

			public Component(IPage page, string xpath) : base(page)
			{
				Xpath = xpath;
			}

			public override bool IsVisible()
			{
				throw new NotImplementedException();
			}

			public override bool IsNotVisible()
			{
				throw new NotImplementedException();
			}

			public override bool HasClass(string className)
			{
				throw new NotImplementedException();
			}

			public override bool IsDisabled()
			{
				throw new NotImplementedException();
			}

			public override T GetValue<T>()
			{
				throw new NotImplementedException();
			}

			public override void Click(int sleepTimeout = 0)
			{
				throw new NotImplementedException();
			}
		}

		private class Page : PageBase
		{
		}
	}
}
