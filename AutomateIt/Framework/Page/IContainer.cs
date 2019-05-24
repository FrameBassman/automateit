namespace AutomateIt.Framework.Page
{
	public interface IContainer : IComponent
	{
		string InnerScss(string relativeScss, params object[] args);
	}
}