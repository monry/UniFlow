using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color", (int) ConnectorType.ValueProviderColor)]
    public class Color : Base<UnityEngine.Color>
    {
        [SerializeField] private UnityEngine.Color value = default;
        private UnityEngine.Color Value => value;

        [SerializeField] private PublishObjectEvent publisher = default;
        [ValuePublisher("Value", ValueInjectionType.Color)]
        private PublishObjectEvent Publisher => publisher ?? (publisher = new PublishObjectEvent());

        protected override UnityEngine.Color Provide()
        {
            var publishValue = ScriptableObject.CreateInstance<ColorObject>();
            publishValue.Value = Value;
            Publisher.Invoke(publishValue);
            return Value;
        }
    }
}
