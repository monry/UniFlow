using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color32", (int) ConnectorType.ValueProviderColor32)]
    public class Color32 : Base<UnityEngine.Color32>
    {
        [SerializeField] private UnityEngine.Color32 value = default;
        private UnityEngine.Color32 Value => value;

        [SerializeField] private PublishColor32Event publisher = default;
        [ValuePublisher("Value")]
        private PublishColor32Event Publisher => publisher ?? (publisher = new PublishColor32Event());

        protected override UnityEngine.Color32 Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
