using System;
using JetBrains.Annotations;

namespace UniFlow.Signal
{
    [PublicAPI]
    public abstract class SignalBase<TSignal> : ISignal<TSignal>
        where TSignal : SignalBase<TSignal>, new()
    {
        public override bool Equals(object obj)
        {
            return obj is TSignal;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        bool IEquatable<TSignal>.Equals(TSignal other)
        {
            return true;
        }

        public static TSignal Create()
        {
            return new TSignal();
        }

        public static bool operator ==(SignalBase<TSignal> first, SignalBase<TSignal> second)
        {
            return Equals(first, second);
        }

        public static bool operator !=(SignalBase<TSignal> first, SignalBase<TSignal> second)
        {
            return !Equals(first, second);
        }
    }

    [Serializable]
    [PublicAPI]
    public abstract class SignalBase<TSignal, TParameter> : ISignal<TSignal>
        where TSignal : SignalBase<TSignal, TParameter>, new()
    {
        protected abstract TParameter ComparableValue { get; set; }

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

        bool IEquatable<TSignal>.Equals(TSignal other)
        {
            return Equals(other);
        }

        public static TSignal Create(TParameter comparableValue)
        {
            return new TSignal
            {
                ComparableValue = comparableValue,
            };
        }

        public static bool operator ==(SignalBase<TSignal, TParameter> first, SignalBase<TSignal, TParameter> second)
        {
            return Equals(first, second);
        }

        public static bool operator !=(SignalBase<TSignal, TParameter> first, SignalBase<TSignal, TParameter> second)
        {
            return !Equals(first, second);
        }
    }
}
