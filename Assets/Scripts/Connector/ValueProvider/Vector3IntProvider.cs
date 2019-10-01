using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3Int", (int) ConnectorType.ValueProviderVector3Int)]
    public class Vector3IntProvider : ProviderBase<Vector3Int>
    {
        [SerializeField] private Vector3Int value = default;
        private Vector3Int Value => value;

        [SerializeField] private PublishVector3IntEvent publisher = default;
        [ValuePublisher("Value")] protected override UnityEvent<Vector3Int> Publisher => publisher ?? (publisher = new PublishVector3IntEvent());

        protected override Vector3Int Provide()
        {
            return Value;
        }
    }
}
