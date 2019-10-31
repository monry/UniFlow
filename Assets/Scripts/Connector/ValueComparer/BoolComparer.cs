using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    [AddComponentMenu("UniFlow/ValueComparer/BoolComparer", (int) ConnectorType.ValueComparerBool)]
    public class BoolComparer : ComparerBase<bool, BoolCollector>
    {
        protected override bool Compare(bool actual)
        {
            return Expect == actual;
        }
    }
}
