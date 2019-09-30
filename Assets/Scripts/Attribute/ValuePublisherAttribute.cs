using System;

namespace UniFlow.Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ValuePublisherAttribute : System.Attribute
    {
        public ValuePublisherAttribute(string name, ValueInjectionType type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public ValueInjectionType Type { get; }
    }
}
