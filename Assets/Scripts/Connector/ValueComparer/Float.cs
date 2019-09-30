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
                    return Mathf.Approximately(Value, compareValue);
                case OperatorType.NotEqual:
                    return !Mathf.Approximately(Value, compareValue);
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