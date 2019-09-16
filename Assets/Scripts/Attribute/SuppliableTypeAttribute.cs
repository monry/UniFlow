using System;
using System.Collections.Generic;

namespace UniFlow.Attribute
{
    public class SuppliableTypeAttribute : System.Attribute
    {
        public SuppliableTypeAttribute(params Type[] types)
        {
            Types = types;
        }

        public IEnumerable<Type> Types { get; }
    }
}