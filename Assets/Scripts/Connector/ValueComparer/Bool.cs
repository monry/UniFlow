using System;
using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    [AddComponentMenu("UniFlow/ValueComparer/Bool", (int) ConnectorType.ValueComparerBool)]
    public class Bool : ComparerBase<bool, Bool.OperatorType>
    {
        protected override bool Compare(bool compareValue)
        {
            switch (Operator)
            {
                case OperatorType.True:
                    return compareValue;
                case OperatorType.False:
                    return !compareValue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public enum OperatorType
        {
            True,
            False,
        }
    }
}