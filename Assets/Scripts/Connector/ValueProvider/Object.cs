using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Object", (int) ConnectorType.ValueProviderObject)]
    public class Object : ValueProviderBase<UnityEngine.Object>
    {
        [SerializeField] private UnityEngine.Object value = default;
        private UnityEngine.Object Value => value;

        [SerializeField] [ValuePublisher("Value", ValueInjectionType.Object)] private PublishObjectEvent publisher = default;
        private PublishObjectEvent Publisher => publisher ?? (publisher = new PublishObjectEvent());

        protected override UnityEngine.Object Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
