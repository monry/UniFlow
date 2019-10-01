using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3", (int) ConnectorType.ValueProviderVector3)]
    public class Vector3Provider : ProviderBase<Vector3>
    {
        [SerializeField] private Vector3 value = default;
        private Vector3 Value => value;

        [SerializeField] private PublishVector3Event publisher = default;
        [ValuePublisher("Value")] protected override UnityEvent<Vector3> Publisher => publisher ?? (publisher = new PublishVector3Event());

        protected override Vector3 Provide()
        {
            return Value;
        }
    }
}
