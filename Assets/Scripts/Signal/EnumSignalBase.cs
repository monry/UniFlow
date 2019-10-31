using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UniFlow.Signal
{
    [Serializable]
    [PublicAPI]
    public abstract class EnumSignalBase<TSignal, TEnum> : SignalBase<TSignal, TEnum>
        where TSignal : EnumSignalBase<TSignal, TEnum>, new()
        where TEnum : Enum
    {
        [SerializeField] private TEnum enumValue = default;

        protected override TEnum ComparableValue
        {
            get => enumValue;
            set => enumValue = value;
        }
    }

    [PublicAPI]
    public abstract class EnumScriptableObjectSignalBase<TSignal, TEnum> : ScriptableObject, ISignal<EnumScriptableObjectSignalBase<TSignal, TEnum>>
        where TSignal : EnumScriptableObjectSignalBase<TSignal, TEnum>
        where TEnum : Enum
    {
        [SerializeField] private TEnum enumValue = default;

        public TEnum EnumValue
        {
            get => enumValue;
            set => enumValue = value;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EnumScriptableObjectSignalBase<TSignal, TEnum> enumScriptableObjectSignal))
            {
                return false;
            }

            return Equals(EnumValue, enumScriptableObjectSignal.EnumValue);
        }

        public override int GetHashCode()
        {
            return EnumValue.GetHashCode();
        }

        bool IEquatable<EnumScriptableObjectSignalBase<TSignal, TEnum>>.Equals(EnumScriptableObjectSignalBase<TSignal, TEnum> other)
        {
            return Equals(other);
        }

        public static bool operator ==(EnumScriptableObjectSignalBase<TSignal, TEnum> left, EnumScriptableObjectSignalBase<TSignal, TEnum> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EnumScriptableObjectSignalBase<TSignal, TEnum> left, EnumScriptableObjectSignalBase<TSignal, TEnum> right)
        {
            return !Equals(left, right);
        }

        public static TSignal Create(TEnum enumValue)
        {
            var signal = CreateInstance<TSignal>();
            signal.EnumValue = enumValue;
            return signal;
        }
    }
}
