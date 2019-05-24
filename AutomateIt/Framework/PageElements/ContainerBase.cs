using AutomateIt.Framework.Page;
using AutomateIt.SmartSelector;
using OpenQA.Selenium;

namespace AutomateIt.Framework.PageElements
{
	public abstract class ContainerBase : ComponentBase, IContainer
	{
		private string _rootScss;

		protected ContainerBase(IPage parent)
			: this(parent, null)
		{
		}

		protected ContainerBase(IPage parent, string rootScss)
			: base(parent)
		{
			_rootScss = rootScss;
		}

		protected virtual string RootScss => _rootScss ?? (_rootScss = "html");

		protected By RootSelector => ScssBuilder.CreateBy(RootScss);

		/// <summary>
		///     Получает Scss для вложенного элемента
		/// </summary>
		public string InnerScss(string relativeScss, params object[] args)
		{
			relativeScss = string.Format(relativeScss, args);
			return ScssBuilder.Concat(RootScss, relativeScss).Value;
		}

		/// <summary>
		///     Получает селектор для вложенного элемента
		/// </summary>
		public By InnerSelector(string relativeScss, params object[] args)
		{
			relativeScss = string.Format(relativeScss, args);
			return ScssBuilder.Concat(RootScss, relativeScss).By;
		}

		/// <summary>
		///     Получает селектор для вложенного элемента
		/// </summary>
		public By InnerSelector(Scss innerScss)
		{
			var rootScss = ScssBuilder.Create(RootScss);
			return rootScss.Concat(innerScss).By;
		}
	}
}