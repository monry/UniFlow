using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/String", (int) ConnectorType.ValueProviderString)]
    public class StringProvider : ProviderBase<string, PublishStringEvent>
    {
    }
}
