namespace AutomateIt.Extensions.Extensions
{
    using System;

    public class StringValueAttribute : Attribute
    {
        public StringValueAttribute(string value)
        {
            this.Value = value;
        }

        public string Value { get; }
    }
}