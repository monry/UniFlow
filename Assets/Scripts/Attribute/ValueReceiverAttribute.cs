using System;

namespace UniFlow.Attribute
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValueReceiverAttribute : System.Attribute
    {
        public ValueReceiverAttribute(string name, ValueInjectionType type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public ValueInjectionType Type { get; }
    }
}
