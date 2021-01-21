using System;
using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    [AddComponentMenu("UniFlow/ValueComparer/ObjectComparer", (int) ConnectorType.ValueComparerObject)]
    public class ObjectComparer : ComparerBase<UnityEngine.Object, ObjectComparer.OperatorType, ObjectCollector>
    {
        protected override bool Compare(UnityEngine.Object actual)
        {
            switch (Operator)
            {
                case OperatorType.Equal:
                    return Equals(actual, Expect);
                case OperatorType.NotEqual:
                    return !Equals(actual, Expect);
                case OperatorType.Null:
                    return actual == default;
                case OperatorType.NotNull:
                    return actual != default;
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
