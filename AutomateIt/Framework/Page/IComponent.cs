namespace AutomateIt.Framework.Page
{
	public interface IComponent : IPageObject
	{
		IPage ParentPage { get; }
		string ComponentName { get; set; }
		bool IsVisible();
	}
}