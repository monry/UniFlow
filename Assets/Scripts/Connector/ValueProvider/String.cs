using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/String", (int) ConnectorType.ValueProviderString)]
    public class String : ValueProviderBase<string>
    {
        [SerializeField] private string value = default;
        private string Value => value;

        [SerializeField] [ValuePublisher("Value", ValueInjectionType.String)] private PublishStringEvent publisher = default;
        private PublishStringEvent Publisher => publisher ?? (publisher = new PublishStringEvent());

        protected override string Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
