using System;
using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    [AddComponentMenu("UniFlow/ValueComparer/Int", (int) ConnectorType.ValueComparerInt)]
    public class IntComparer : ComparerBase<int, IntComparer.OperatorType>
    {
        protected override bool Compare(int compareValue)
        {
            switch (Operator)
            {
                case OperatorType.Equal:
                    return compareValue == Expect;
                case OperatorType.NotEqual:
                    return compareValue != Expect;
                case OperatorType.Greater:
                    return compareValue > Expect;
                case OperatorType.GreaterOrEqual:
                    return compareValue >= Expect;
                case OperatorType.Less:
                    return compareValue < Expect;
                case OperatorType.LessOrEqual:
                    return compareValue <= Expect;
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
