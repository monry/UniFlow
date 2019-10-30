using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UniFlow.Signal
{
    [PublicAPI]
    public abstract class ScriptableObjectSignalBase<TSignal> : ScriptableObject, ISignal<ScriptableObjectSignalBase<TSignal>>
        where TSignal : ScriptableObjectSignalBase<TSignal>
    {
        public override bool Equals(object obj)
        {
            return obj is TSignal;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        bool IEquatable<ScriptableObjectSignalBase<TSignal>>.Equals(ScriptableObjectSignalBase<TSignal> other)
        {
            return Equals(other);
        }

        public static TSignal Create()
        {
            return CreateInstance<TSignal>();
        }

        public static bool operator ==(ScriptableObjectSignalBase<TSignal> left, ScriptableObjectSignalBase<TSignal> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ScriptableObjectSignalBase<TSignal> left, ScriptableObjectSignalBase<TSignal> right)
        {
            return !Equals(left, right);
        }
    }

    [PublicAPI]
    public abstract class ScriptableObjectSignalBase<TSignal, TComparableValue> : ScriptableObject, ISignal<ScriptableObjectSignalBase<TSignal, TComparableValue>>
        where TSignal : ScriptableObjectSignalBase<TSignal, TComparableValue>
    {
        protected abstract TComparableValue ComparableValue { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is TSignal signal))
            {
                return false;
            }

            return Equals(ComparableValue, signal.ComparableValue);
        }

        public override int GetHashCode()
        {
            return ComparableValue.GetHashCode();
        }

        bool IEquatable<ScriptableObjectSignalBase<TSignal, TComparableValue>>.Equals(ScriptableObjectSignalBase<TSignal, TComparableValue> other)
        {
            return Equals(other);
        }

        public static TSignal Create(TComparableValue comparableValue)
        {
            var signal = CreateInstance<TSignal>();
            signal.ComparableValue = comparableValue;
            return signal;
        }

        public static bool operator ==(ScriptableObjectSignalBase<TSignal, TComparableValue> left, ScriptableObjectSignalBase<TSignal, TComparableValue> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ScriptableObjectSignalBase<TSignal, TComparableValue> left, ScriptableObjectSignalBase<TSignal, TComparableValue> right)
        {
            return !Equals(left, right);
        }
    }
}
