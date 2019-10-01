using System;
using JetBrains.Annotations;

namespace UniFlow.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    [MeansImplicitUse]
    public class ValuePublisherAttribute : System.Attribute
    {
        public ValuePublisherAttribute(string name = "")
        {
            Name = name;
        }

        public string Name { get; }
    }
}
