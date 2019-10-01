using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector4", (int) ConnectorType.ValueProviderVector4)]
    public class Vector4Provider : ProviderBase<Vector4>
    {
        [SerializeField] private Vector4 value = default;
        private Vector4 Value => value;

        [SerializeField] private PublishVector4Event publisher = default;
        [ValuePublisher("Value")] protected override UnityEvent<Vector4> Publisher => publisher ?? (publisher = new PublishVector4Event());

        protected override Vector4 Provide()
        {
            return Value;
        }
    }
}
