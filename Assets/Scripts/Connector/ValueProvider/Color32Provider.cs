using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color32", (int) ConnectorType.ValueProviderColor32)]
    public class Color32Provider : ProviderBase<Color32>
    {
        [SerializeField] private Color32 value = default;
        private Color32 Value => value;

        [SerializeField] private PublishColor32Event publisher = default;
        [ValuePublisher("Value")] protected override UnityEvent<Color32> Publisher => publisher ?? (publisher = new PublishColor32Event());

        protected override Color32 Provide()
        {
            return Value;
        }
    }
}
