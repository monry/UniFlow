using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Float", (int) ConnectorType.ValueProviderFloat)]
    public class FloatProvider : ValueProviderBase<float, PublishFloatEvent>
    {
        protected override float Provide()
        {
            return Value;
        }
    }
}
