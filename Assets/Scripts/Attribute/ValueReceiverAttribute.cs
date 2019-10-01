using System;
using JetBrains.Annotations;

namespace UniFlow.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    [MeansImplicitUse]
    [PublicAPI]
    public class ValueReceiverAttribute : System.Attribute
    {
        public ValueReceiverAttribute() : this("")
        {
        }

        public ValueReceiverAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
