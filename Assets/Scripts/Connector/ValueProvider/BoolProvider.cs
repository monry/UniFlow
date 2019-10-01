using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Bool", (int) ConnectorType.ValueProviderBool)]
    public class BoolProvider : ProviderBase<bool>
    {
        [SerializeField] private bool value = default;
        private bool Value => value;

        [SerializeField] private PublishBoolEvent publisher = default;
        [ValuePublisher("Value")] protected override UnityEvent<bool> Publisher => publisher ?? (publisher = new PublishBoolEvent());

        protected override bool Provide()
        {
            return Value;
        }
    }
}
