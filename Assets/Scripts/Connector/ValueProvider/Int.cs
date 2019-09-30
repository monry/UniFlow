using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Int", (int) ConnectorType.ValueProviderInt)]
    public class Int : Base<int>
    {
        [SerializeField] private int value = default;
        private int Value => value;

        [SerializeField] [ValuePublisher("Value", ValueInjectionType.Int)] private PublishIntEvent publisher = default;
        private PublishIntEvent Publisher => publisher ?? (publisher = new PublishIntEvent());

        protected override int Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
