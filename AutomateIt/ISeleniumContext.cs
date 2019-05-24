namespace AutomateIt
{
    using AutomateIt.Framework.Browser;
    using AutomateIt.Framework.Service;

    public interface ISeleniumContext
    {
        Web Web { get; }
        Browser Browser { get; }
    }
}