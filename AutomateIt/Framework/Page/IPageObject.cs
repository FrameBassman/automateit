using AutomateIt.Framework.Browser;
using AutomateIt.Logging;

namespace AutomateIt.Framework.Page
{
	public interface IPageObject
	{
		Browser.Browser Browser { get; }
		ITestLogger Log { get; }

		BrowserAction Action { get; }
		BrowserAlert Alert { get; }
		BrowserFind Find { get; }
		BrowserGet Get { get; }
		BrowserGo Go { get; }
		BrowserIs Is { get; }
		BrowserState State { get; }
		BrowserWait Wait { get; }
		BrowserJs Js { get; }
		BrowserWindow Window { get; }
		BrowserCookies Cookies { get; }
	}
}