using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Quaternion", (int) ConnectorType.ValueProviderQuaternion)]
    public class Quaternion : Base<UnityEngine.Quaternion>
    {
        [SerializeField] private UnityEngine.Quaternion value = default;
        private UnityEngine.Quaternion Value => value;

        [SerializeField] private PublishQuaternionEvent publisher = default;
        [ValuePublisher("Value")]
        private PublishQuaternionEvent Publisher => publisher ?? (publisher = new PublishQuaternionEvent());

        protected override UnityEngine.Quaternion Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
