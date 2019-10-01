using System;
using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    [AddComponentMenu("UniFlow/ValueComparer/Float", (int) ConnectorType.ValueComparerFloat)]
    public class Float : Base<float, Float.OperatorType>
    {
        protected override bool Compare(float compareValue)
        {
            switch (Operator)
            {
                case OperatorType.Equal:
                    return Mathf.Approximately(Expect, compareValue);
                case OperatorType.NotEqual:
                    return !Mathf.Approximately(Expect, compareValue);
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
