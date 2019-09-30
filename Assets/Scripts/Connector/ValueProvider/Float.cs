using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Float", (int) ConnectorType.ValueProviderFloat)]
    public class Float : Base<float>
    {
        [SerializeField] private float value = default;
        private float Value => value;

        [SerializeField] private PublishFloatEvent publisher = default;
        [ValuePublisher("Value")]
        private PublishFloatEvent Publisher => publisher ?? (publisher = new PublishFloatEvent());

        protected override float Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
