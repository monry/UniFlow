using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniFlow.Signal
{
    [Serializable]
    [PublicAPI]
    public struct StringSignal : IEquatableSignal<StringSignal>
    {
        public StringSignal(string name)
        {
            this.name = name;
            parameter = default;
        }

        public StringSignal(string name, SignalParameter parameter)
        {
            this.name = name;
            this.parameter = parameter;
        }

        [SerializeField] private string name;
        [SerializeField] private SignalParameter parameter;

        public string Name
        {
            get => name;
            set => name = value;
        }
        public SignalParameter Parameter
        {
            get => parameter;
            set => parameter = value;
        }

        public bool Equals(StringSignal other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is StringSignal other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }

        [Serializable]
        [PublicAPI]
        public struct SignalParameter
        {
            public SignalParameter(bool boolValue, int intValue, float floatValue, string stringValue, Object objectValue, ScriptableObject scriptableObjectValue)
            {
                this.boolValue = boolValue;
                this.intValue = intValue;
                this.floatValue = floatValue;
                this.stringValue = stringValue;
                this.objectValue = objectValue;
                this.scriptableObjectValue = scriptableObjectValue;
            }

            [SerializeField] private bool boolValue;
            [SerializeField] private int intValue;
            [SerializeField] private float floatValue;
            [SerializeField] private string stringValue;
            [SerializeField] private Object objectValue;
            [SerializeField] private ScriptableObject scriptableObjectValue;

            public bool BoolValue
            {
                get => boolValue;
                set => boolValue = value;
            }
            public int IntValue
            {
                get => intValue;
                set => intValue = value;
            }
            public float FloatValue
            {
                get => floatValue;
                set => floatValue = value;
            }
            public string StringValue
            {
                get => stringValue;
                set => stringValue = value;
            }
            public Object ObjectValue
            {
                get => objectValue;
                set => objectValue = value;
            }
            public ScriptableObject ScriptableObjectValue
            {
                get => scriptableObjectValue;
                set => scriptableObjectValue = value;
            }
        }
    }
}
