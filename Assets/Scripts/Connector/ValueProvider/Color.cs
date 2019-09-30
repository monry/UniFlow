using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color", (int) ConnectorType.ValueProviderColor)]
    public class Color : Base<UnityEngine.Color>
    {
        [SerializeField] private UnityEngine.Color value = default;
        private UnityEngine.Color Value => value;

        [SerializeField] private PublishColorEvent publisher = default;
        [ValuePublisher("Value")]
        private PublishColorEvent Publisher => publisher ?? (publisher = new PublishColorEvent());

        protected override UnityEngine.Color Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
