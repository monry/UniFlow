using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2", (int) ConnectorType.ValueProviderVector2)]
    public class Vector2 : ValueProviderBase<UnityEngine.Vector2>
    {
        [SerializeField] private UnityEngine.Vector2 value = default;
        private UnityEngine.Vector2 Value => value;

        [SerializeField] [ValuePublisher("Value", ValueInjectionType.Vector2)] private PublishObjectEvent publisher = default;
        private PublishObjectEvent Publisher => publisher ?? (publisher = new PublishObjectEvent());

        protected override UnityEngine.Vector2 Provide()
        {
            var publishValue = ScriptableObject.CreateInstance<Vector2Object>();
            publishValue.Value = Value;
            Publisher.Invoke(publishValue);
            return Value;
        }
    }
}
