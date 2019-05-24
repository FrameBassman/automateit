using System.Collections.Generic;
using System.Collections.Specialized;
using AutomateIt.Framework.Browser;
using AutomateIt.Framework.Service;
using AutomateIt.Logging;
using OpenQA.Selenium;

namespace AutomateIt.Framework.Page
{
	public interface IPage : IPageObject
	{
		/// <summary>
		///     ����, ������� ������� �� ���������� ��������
		/// </summary>
		new List<Cookie> Cookies { get; set; }

		/// <summary>
		///     ������, ������������ � Url
		/// </summary>
		Dictionary<string, string> Data { get; set; }

		/// <summary>
		///     ���������, ������������ � Url
		/// </summary>
		Dictionary<string, string> Params { get; set; }

		/// <summary>
		///     ���������� � ������, ��������� � ���������� ����
		/// </summary>
		BaseUrlInfo BaseUrlInfo { get; set; }

		List<IHtmlAlert> Alerts { get; }

		List<IOverlay> Overlays{ get; }

		/// <summary>
		///     ������ ������������������ ����������
		/// </summary>
		List<IProgressBar> ProgressBars { get; }
        
		/// <summary>
		/// Browser Options to use on the page
		/// </summary>
		BrowserOptions BrowserOptions { get; }

		bool Invalidated { get; set; }

		/// <summary>
		///     ���������������� ���������
		/// </summary>
		void RegisterComponent(IComponent component);

		/// <summary>
		///     ���������������� ���������
		/// </summary>
		T RegisterComponent<T>(string componentName, params object[] args) where T : IComponent;

		/// <summary>
		///     ������� ���������
		/// </summary>
		T CreateComponent<T>(params object[] args) where T : IComponent;

		/// <summary>
		///     �������������� ��������
		/// </summary>
		void Activate(Browser.Browser browser, ITestLogger log);

		/// <summary>
		/// Initialize page components
		/// </summary>
		void InitializeComponents();

		void CleanUp();
		void Refresh();
	}
}