using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/String", (int) ConnectorType.ValueProviderString)]
    public class String : Base<string>
    {
        [SerializeField] private string value = default;
        private string Value => value;

        [SerializeField] private PublishStringEvent publisher = default;
        [ValuePublisher("Value")]
        private PublishStringEvent Publisher => publisher ?? (publisher = new PublishStringEvent());

        protected override string Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
