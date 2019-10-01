using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color", (int) ConnectorType.ValueProviderColor)]
    public class ColorProvider : ProviderBase<Color>
    {
        [SerializeField] private Color value = default;
        private Color Value => value;

        [SerializeField] private PublishColorEvent publisher = default;
        [ValuePublisher("Value")] protected override UnityEvent<Color> Publisher => publisher ?? (publisher = new PublishColorEvent());

        protected override Color Provide()
        {
            return Value;
        }
    }
}
