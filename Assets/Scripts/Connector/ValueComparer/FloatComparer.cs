using System;
using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    [AddComponentMenu("UniFlow/ValueComparer/Float", (int) ConnectorType.ValueComparerFloat)]
    public class FloatComparer : ComparerBase<float, FloatComparer.OperatorType, FloatCollector>
    {
        protected override bool Compare(float actual)
        {
            switch (Operator)
            {
                case OperatorType.Equal:
                    return Mathf.Approximately(Expect, actual);
                case OperatorType.NotEqual:
                    return !Mathf.Approximately(Expect, actual);
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
