namespace AutomateIt.SmartSelector
{
	internal class ScssAttribute
	{
		public AttributeMatchStyle MatchStyle;

		public string Name;

		public string Value;

		public ScssAttribute(string name, string value, AttributeMatchStyle matchStyle)
		{
			this.Name = name;
			this.Value = value;
			this.MatchStyle = matchStyle;
		}
	}
}