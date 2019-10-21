namespace UniFlow.Signal
{
    public abstract class SignalBase<TSignal, TComparableValue>
        where TSignal : SignalBase<TSignal, TComparableValue>
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
