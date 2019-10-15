using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Bool", (int) ConnectorType.ValueProviderBool)]
    public class BoolProvider : ProviderBase<bool, PublishBoolEvent, BoolCollector>
    {
    }
}
