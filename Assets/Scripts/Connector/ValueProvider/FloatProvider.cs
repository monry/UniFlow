using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Float", (int) ConnectorType.ValueProviderFloat)]
    public class FloatProvider : ProviderBase<float, PublishFloatEvent>
    {
        [SerializeField] private float value = default;
        private float Value => value;

        protected override float Provide()
        {
            return Value;
        }
    }
}
