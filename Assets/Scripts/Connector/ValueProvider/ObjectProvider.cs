using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Object", (int) ConnectorType.ValueProviderObject)]
    public class ObjectProvider : ProviderBase<Object>
    {
        [SerializeField] private Object value = default;
        private Object Value => value;

        [SerializeField] private PublishObjectEvent publisher = default;
        [ValuePublisher("Value")] protected override UnityEvent<Object> Publisher => publisher ?? (publisher = new PublishObjectEvent());

        protected override Object Provide()
        {
            return Value;
        }
    }
}
