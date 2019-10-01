using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2", (int) ConnectorType.ValueProviderVector2)]
    public class Vector2Provider : ProviderBase<Vector2>
    {
        [SerializeField] private Vector2 value = default;
        private Vector2 Value => value;

        [SerializeField] private PublishVector2Event publisher = default;
        [ValuePublisher("Value")] protected override UnityEvent<Vector2> Publisher => publisher ?? (publisher = new PublishVector2Event());

        protected override Vector2 Provide()
        {
            return Value;
        }
    }
}
