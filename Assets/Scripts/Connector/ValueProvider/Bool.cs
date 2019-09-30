using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Bool", (int) ConnectorType.ValueProviderBool)]
    public class Bool : Base<bool>
    {
        [SerializeField] private bool value = default;
        private bool Value => value;

        [SerializeField] private PublishBoolEvent publisher = default;
        [ValuePublisher("Value")] private PublishBoolEvent Publisher => publisher ?? (publisher = new PublishBoolEvent());

        protected override bool Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
