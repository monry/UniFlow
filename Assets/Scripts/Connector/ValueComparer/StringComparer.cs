using System;
using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    [AddComponentMenu("UniFlow/ValueComparer/String", (int) ConnectorType.ValueComparerString)]
    public class StringComparer : ComparerBase<string, StringComparer.OperatorType>
    {
        protected override bool Compare(string actual)
        {
            switch (Operator)
            {
                case OperatorType.Equal:
                    return actual == Expect;
                case OperatorType.NotEqual:
                    return actual != Expect;
                case OperatorType.NullOrEmpty:
                    return string.IsNullOrEmpty(actual);
                case OperatorType.NotNullOrEmpty:
                    return !string.IsNullOrEmpty(actual);
                case OperatorType.NullOrWhitespace:
                    return string.IsNullOrWhiteSpace(actual);
                case OperatorType.NotNullOrWhitespace:
                    return !string.IsNullOrWhiteSpace(actual);
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
