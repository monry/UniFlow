using System;
using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    [AddComponentMenu("UniFlow/ValueComparer/Int", (int) ConnectorType.ValueComparerInt)]
    public class Int : ComparerBase<int, Int.OperatorType>
    {
        protected override bool Compare(int compareValue)
        {
            switch (Operator)
            {
                case OperatorType.Equal:
                    return compareValue == Value;
                case OperatorType.NotEqual:
                    return compareValue != Value;
                case OperatorType.Greater:
                    return compareValue > Value;
                case OperatorType.GreaterOrEqual:
                    return compareValue >= Value;
                case OperatorType.Less:
                    return compareValue < Value;
                case OperatorType.LessOrEqual:
                    return compareValue <= Value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public enum OperatorType
        {
            Equal,
            NotEqual,
            Greater,
            GreaterOrEqual,
            Less,
            LessOrEqual,
        }
    }
}
