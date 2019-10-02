using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    [AddComponentMenu("UniFlow/ValueComparer/Bool", (int) ConnectorType.ValueComparerBool)]
    public class BoolComparer : ComparerBase<bool>
    {
        protected override bool Compare(bool actual)
        {
            return Expect == actual;
        }
    }
}
