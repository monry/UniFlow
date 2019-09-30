using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector4", (int) ConnectorType.ValueProviderVector4)]
    public class Vector4 : Base<UnityEngine.Vector4>
    {
        [SerializeField] private UnityEngine.Vector4 value = default;
        private UnityEngine.Vector4 Value => value;

        [SerializeField] private PublishVector4Event publisher = default;
        [ValuePublisher("Value")]
        private PublishVector4Event Publisher => publisher ?? (publisher = new PublishVector4Event());

        protected override UnityEngine.Vector4 Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
