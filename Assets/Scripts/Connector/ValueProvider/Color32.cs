using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color32", (int) ConnectorType.ValueProviderColor32)]
    public class Color32 : ValueProviderBase<UnityEngine.Color32>
    {
        [SerializeField] private UnityEngine.Color32 value = default;
        private UnityEngine.Color32 Value => value;

        [SerializeField] [ValuePublisher("Value", ValueInjectionType.Color32)] private PublishObjectEvent publisher = default;
        private PublishObjectEvent Publisher => publisher ?? (publisher = new PublishObjectEvent());

        protected override UnityEngine.Color32 Provide()
        {
            var publishValue = ScriptableObject.CreateInstance<Color32Object>();
            publishValue.Value = Value;
            Publisher.Invoke(publishValue);
            return Value;
        }
    }
}
