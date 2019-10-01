using System;
using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    [AddComponentMenu("UniFlow/ValueComparer/Object", (int) ConnectorType.ValueComparerObject)]
    public class Object : Base<UnityEngine.Object, Object.OperatorType>
    {
        protected override bool Compare(UnityEngine.Object compareValue)
        {
            switch (Operator)
            {
                case OperatorType.Equal:
                    return compareValue == Expect;
                case OperatorType.NotEqual:
                    return compareValue != Expect;
                case OperatorType.Null:
                    return compareValue == default;
                case OperatorType.NotNull:
                    return compareValue != default;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public enum OperatorType
        {
            Equal,
            NotEqual,
            Null,
            NotNull,
        }
    }
}
