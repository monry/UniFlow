using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Quaternion", (int) ConnectorType.ValueProviderQuaternion)]
    public class Quaternion : Base<UnityEngine.Quaternion>
    {
        [SerializeField] private UnityEngine.Quaternion value = default;
        private UnityEngine.Quaternion Value => value;

        [SerializeField] private PublishObjectEvent publisher = default;
        [ValuePublisher("Value", ValueInjectionType.Quaternion)]
        private PublishObjectEvent Publisher => publisher ?? (publisher = new PublishObjectEvent());

        protected override UnityEngine.Quaternion Provide()
        {
            var publishValue = ScriptableObject.CreateInstance<QuaternionObject>();
            publishValue.Value = Value;
            Publisher.Invoke(publishValue);
            return Value;
        }
    }
}
