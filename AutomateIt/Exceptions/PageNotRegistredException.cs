namespace AutomateIt.Exceptions
{
    using System;
    using AutomateIt.Framework.Page;

    public class PageNotRegisteredException : Exception
    {
        public PageNotRegisteredException(IPage page)
            : base(string.Format("There are not services with registered page of type {0}", page.GetType().Name))
        {
        }
    }
}