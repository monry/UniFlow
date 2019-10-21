using UnityEngine;

namespace UniFlow.Signal
{
    public abstract class ScriptableObjectSignalBase : ScriptableObject
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType();
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }

    public abstract class ScriptableObjectSignalBase<TSignal, TComparableValue> : ScriptableObject
        where TSignal : ScriptableObjectSignalBase<TSignal, TComparableValue>
    {
        protected abstract TComparableValue CreateComparableValue();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals(((TSignal) obj).CreateComparableValue(), CreateComparableValue());
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
