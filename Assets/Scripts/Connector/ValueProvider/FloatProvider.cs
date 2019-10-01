using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Float", (int) ConnectorType.ValueProviderFloat)]
    public class FloatProvider : ProviderBase<float>
    {
        [SerializeField] private float value = default;
        private float Value => value;

        [SerializeField] private PublishFloatEvent publisher = default;
        [ValuePublisher("Value")] protected override UnityEvent<float> Publisher => publisher ?? (publisher = new PublishFloatEvent());

        protected override float Provide()
        {
            return Value;
        }
    }
}
