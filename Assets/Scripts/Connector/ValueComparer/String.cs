using System;
using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    [AddComponentMenu("UniFlow/ValueComparer/String", (int) ConnectorType.ValueComparerString)]
    public class String : Base<string, String.OperatorType>
    {
        protected override bool Compare(string compareValue)
        {
            switch (Operator)
            {
                case OperatorType.Equal:
                    return compareValue == Value;
                case OperatorType.NotEqual:
                    return compareValue != Value;
                case OperatorType.NullOrEmpty:
                    return string.IsNullOrEmpty(compareValue);
                case OperatorType.NotNullOrEmpty:
                    return !string.IsNullOrEmpty(compareValue);
                case OperatorType.NullOrWhitespace:
                    return string.IsNullOrWhiteSpace(compareValue);
                case OperatorType.NotNullOrWhitespace:
                    return !string.IsNullOrWhiteSpace(compareValue);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public enum OperatorType
        {
            Equal,
            NotEqual,
            NullOrEmpty,
            NotNullOrEmpty,
            NullOrWhitespace,
            NotNullOrWhitespace,
        }
    }
}