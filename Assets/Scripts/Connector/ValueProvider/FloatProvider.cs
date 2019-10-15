using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Float", (int) ConnectorType.ValueProviderFloat)]
    public class FloatProvider : ProviderBase<float, PublishFloatEvent>
    {
    }
}
