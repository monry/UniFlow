using System;

namespace UniFlow.Connector.ValueComparer
{
    public abstract class EnumComparerBase<TEnum, TCollector> : ComparerBase<TEnum, EnumComparerBase<TEnum, TCollector>.OperatorType, TCollector>
        where TCollector : ValueCollectorBase<TEnum>, new()
        where TEnum : Enum
    {
        protected override bool Compare(TEnum actual)
        {
            switch (Operator)
            {
                case OperatorType.Equal:
                    return Equals(actual, Expect);
                case OperatorType.NotEqual:
                    return !Equals(actual, Expect);
                case OperatorType.HasFlag:
                    return Expect.HasFlag(actual);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public enum OperatorType
        {
            Equal,
            NotEqual,
            HasFlag,
        }
    }
}
