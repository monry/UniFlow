using System;
using JetBrains.Annotations;

namespace UniFlow.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    [MeansImplicitUse]
    [PublicAPI]
    public class ValuePublisherAttribute : System.Attribute
    {
        public ValuePublisherAttribute() : this("")
        {
        }

        public ValuePublisherAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
