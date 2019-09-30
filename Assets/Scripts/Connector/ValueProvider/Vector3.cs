using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3", (int) ConnectorType.ValueProviderVector3)]
    public class Vector3 : Base<UnityEngine.Vector3>
    {
        [SerializeField] private UnityEngine.Vector3 value = default;
        private UnityEngine.Vector3 Value => value;

        [SerializeField] private PublishVector3Event publisher = default;
        [ValuePublisher("Value")]
        private PublishVector3Event Publisher => publisher ?? (publisher = new PublishVector3Event());

        protected override UnityEngine.Vector3 Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
