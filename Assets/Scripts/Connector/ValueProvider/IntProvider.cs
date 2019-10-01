using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Int", (int) ConnectorType.ValueProviderInt)]
    public class IntProvider : ProviderBase<int>
    {
        [SerializeField] private int value = default;
        private int Value => value;

        [SerializeField] private PublishIntEvent publisher = default;
        [ValuePublisher("Value")] protected override UnityEvent<int> Publisher => publisher ?? (publisher = new PublishIntEvent());

        protected override int Provide()
        {
            return Value;
        }
    }
}
