using System;

namespace UniFlow.Connector.ValueComparer
{
    public abstract class Enum<TEnum> : Base<TEnum, Enum<TEnum>.OperatorType> where TEnum : Enum
    {
        protected override bool Compare(TEnum compareValue)
        {
            switch (Operator)
            {
                case OperatorType.Equal:
                    return Equals(compareValue, Value);
                case OperatorType.NotEqual:
                    return !Equals(compareValue, Value);
                case OperatorType.HasFlag:
                    return Value.HasFlag(compareValue);
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