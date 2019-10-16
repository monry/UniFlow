using System;
using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    [AddComponentMenu("UniFlow/ValueComparer/Int", (int) ConnectorType.ValueComparerInt)]
    public class IntComparer : ComparerBase<int, IntComparer.OperatorType, IntCollector>
    {
        protected override bool Compare(int actual)
        {
            switch (Operator)
            {
                case OperatorType.Equal:
                    return actual == Expect;
                case OperatorType.NotEqual:
                    return actual != Expect;
                case OperatorType.Greater:
                    return actual > Expect;
                case OperatorType.GreaterOrEqual:
                    return actual >= Expect;
                case OperatorType.Less:
                    return actual < Expect;
                case OperatorType.LessOrEqual:
                    return actual <= Expect;
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
