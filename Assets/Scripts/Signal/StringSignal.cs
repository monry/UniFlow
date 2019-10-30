using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniFlow.Signal
{
    [Serializable]
    [PublicAPI]
    public sealed class StringSignal : SignalBase<StringSignal, string>
    {
        [SerializeField] private string signalName;
        [SerializeField] private SignalParameter parameter;

        protected override string ComparableValue
        {
            get => signalName;
            set => signalName = value;
        }

        public SignalParameter Parameter
        {
            get => parameter;
            set => parameter = value;
        }

        public static StringSignal Create(string signalName, SignalParameter signalParameter = default)
        {
            return new StringSignal
            {
                signalName = signalName,
                parameter = signalParameter,
            };
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

    [Serializable]
    public sealed class StringSignalCollector : ValueCollectorBase<StringSignal>
    {
    }
}
