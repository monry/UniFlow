using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector4", (int) ConnectorType.ValueProviderVector4)]
    public class Vector4 : Base<UnityEngine.Vector4>
    {
        [SerializeField] private UnityEngine.Vector4 value = default;
        private UnityEngine.Vector4 Value => value;

        [SerializeField] [ValuePublisher("Value", ValueInjectionType.Vector4)] private PublishObjectEvent publisher = default;
        private PublishObjectEvent Publisher => publisher ?? (publisher = new PublishObjectEvent());

        protected override UnityEngine.Vector4 Provide()
        {
            var publishValue = ScriptableObject.CreateInstance<Vector4Object>();
            publishValue.Value = Value;
            Publisher.Invoke(publishValue);
            return Value;
        }
    }
}
