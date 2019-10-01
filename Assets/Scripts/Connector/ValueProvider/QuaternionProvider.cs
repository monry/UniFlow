using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Quaternion", (int) ConnectorType.ValueProviderQuaternion)]
    public class QuaternionProvider : ProviderBase<Quaternion>
    {
        [SerializeField] private Quaternion value = default;
        private Quaternion Value => value;

        [SerializeField] private PublishQuaternionEvent publisher = default;
        [ValuePublisher("Value")] protected override UnityEvent<Quaternion> Publisher => publisher ?? (publisher = new PublishQuaternionEvent());

        protected override Quaternion Provide()
        {
            return Value;
        }
    }
}
