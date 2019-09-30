using System;

namespace UniFlow.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValueReceiverAttribute : System.Attribute
    {
        public ValueReceiverAttribute(string name = "")
        {
            Name = name;
        }

        public string Name { get; }
    }
}
