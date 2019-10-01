using System;
using JetBrains.Annotations;

namespace UniFlow.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    [MeansImplicitUse]
    public class ValueReceiverAttribute : System.Attribute
    {
        public ValueReceiverAttribute(string name = "")
        {
            Name = name;
        }

        public string Name { get; }
    }
}
