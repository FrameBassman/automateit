namespace AutomateIt.SmartSelector
{
	using Extensions.Extensions;

	internal enum AttributeMatchStyle
	{
		[StringValue("=")]
		Equal,

		[StringValue("~")]
		Contains
	}
}