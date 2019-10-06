using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Bool", (int) ConnectorType.ValueProviderBool)]
    public class BoolProvider : ValueProviderBase<bool, PublishBoolEvent>
    {
        protected override bool Provide()
        {
            return Value;
        }
    }
}
