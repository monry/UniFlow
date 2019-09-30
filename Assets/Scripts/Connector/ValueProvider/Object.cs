using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Object", (int) ConnectorType.ValueProviderObject)]
    public class Object : Base<UnityEngine.Object>
    {
        [SerializeField] private UnityEngine.Object value = default;
        private UnityEngine.Object Value => value;

        [SerializeField] private PublishObjectEvent publisher = default;
        [ValuePublisher("Value", ValueInjectionType.Object)]
        private PublishObjectEvent Publisher => publisher ?? (publisher = new PublishObjectEvent());

        protected override UnityEngine.Object Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
