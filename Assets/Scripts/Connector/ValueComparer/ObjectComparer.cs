using System;
using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    [AddComponentMenu("UniFlow/ValueComparer/Object", (int) ConnectorType.ValueComparerObject)]
    public class ObjectComparer : ComparerBase<UnityEngine.Object, ObjectComparer.OperatorType>
    {
        protected override bool Compare(UnityEngine.Object actual)
        {
            switch (Operator)
            {
                case OperatorType.Equal:
                    return actual == Expect;
                case OperatorType.NotEqual:
                    return actual != Expect;
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
