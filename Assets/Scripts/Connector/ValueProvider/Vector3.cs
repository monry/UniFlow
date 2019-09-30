using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3", (int) ConnectorType.ValueProviderVector3)]
    public class Vector3 : Base<UnityEngine.Vector3>
    {
        [SerializeField] private UnityEngine.Vector3 value = default;
        private UnityEngine.Vector3 Value => value;

        [SerializeField] [ValuePublisher("Value", ValueInjectionType.Vector3)] private PublishObjectEvent publisher = default;
        private PublishObjectEvent Publisher => publisher ?? (publisher = new PublishObjectEvent());

        protected override UnityEngine.Vector3 Provide()
        {
            var publishValue = ScriptableObject.CreateInstance<Vector3Object>();
            publishValue.Value = Value;
            Publisher.Invoke(publishValue);
            return Value;
        }
    }
}
