using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2", (int) ConnectorType.ValueProviderVector2)]
    public class Vector2 : Base<UnityEngine.Vector2>
    {
        [SerializeField] private UnityEngine.Vector2 value = default;
        private UnityEngine.Vector2 Value => value;

        [SerializeField] private PublishVector2Event publisher = default;
        [ValuePublisher("Value")]
        private PublishVector2Event Publisher => publisher ?? (publisher = new PublishVector2Event());

        protected override UnityEngine.Vector2 Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
