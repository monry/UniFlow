using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/RuntimePlatform", (int) ConnectorType.ValueProviderRuntimePlatform)]
    public class RuntimePlatformProvider : ProviderBase<RuntimePlatform, PublishRuntimePlatformEvent, RuntimePlatformCollector>
    {
    }
}
