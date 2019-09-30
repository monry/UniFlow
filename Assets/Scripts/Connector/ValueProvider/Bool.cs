using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Bool", (int) ConnectorType.ValueProviderBool)]
    public class Bool : ValueProviderBase<bool>
    {
        [SerializeField] private bool value = default;
        private bool Value => value;

        [SerializeField] [ValuePublisher("Value", ValueInjectionType.Bool)] private PublishBoolEvent publisher = default;
        private PublishBoolEvent Publisher => publisher ?? (publisher = new PublishBoolEvent());

        protected override bool Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
