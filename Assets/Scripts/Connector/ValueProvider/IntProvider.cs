using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Int", (int) ConnectorType.ValueProviderInt)]
    public class IntProvider : ProviderBase<int, PublishIntEvent, IntCollector>
    {
    }
}
