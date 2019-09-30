using System;

namespace UniFlow.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValuePublisherAttribute : System.Attribute
    {
        public ValuePublisherAttribute(string name = "")
        {
            Name = name;
        }

        public string Name { get; }
    }
}
