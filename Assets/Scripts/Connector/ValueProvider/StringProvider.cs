using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/String", (int) ConnectorType.ValueProviderString)]
    public class StringProvider : ProviderBase<string>
    {
        [SerializeField] private string value = default;
        private string Value => value;

        [SerializeField] private PublishStringEvent publisher = default;
        [ValuePublisher("Value")] protected override UnityEvent<string> Publisher => publisher ?? (publisher = new PublishStringEvent());

        protected override string Provide()
        {
            return Value;
        }
    }
}
