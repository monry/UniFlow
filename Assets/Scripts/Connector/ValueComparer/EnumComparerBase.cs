using System;

namespace UniFlow.Connector.ValueComparer
{
    public abstract class EnumComparerBase<TEnum> : ComparerBase<TEnum, EnumComparerBase<TEnum>.OperatorType> where TEnum : Enum
    {
        protected override bool Compare(TEnum compareValue)
        {
            switch (Operator)
            {
                case OperatorType.Equal:
                    return Equals(compareValue, Expect);
                case OperatorType.NotEqual:
                    return !Equals(compareValue, Expect);
                case OperatorType.HasFlag:
                    return Expect.HasFlag(compareValue);
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
