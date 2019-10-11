using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/RuntimePlatform", (int) ConnectorType.ValueProviderRuntimePlatform)]
    public class RuntimePlatformProvider : ValueProviderBase<RuntimePlatform, PublishRuntimePlatformEvent>
    {
        protected override RuntimePlatform Provide()
        {
            return Application.platform;
        }
    }
}
