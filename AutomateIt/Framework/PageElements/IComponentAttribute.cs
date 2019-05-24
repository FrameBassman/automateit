namespace AutomateIt.Framework.PageElements
{
	public interface IComponentAttribute
	{
		object[] Args { get; }
		string ComponentName { get; set; }
	}
}