using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2Int", (int) ConnectorType.ValueProviderVector2Int)]
    public class Vector2IntProvider : ProviderBase<Vector2Int>
    {
        [SerializeField] private Vector2Int value = default;
        private Vector2Int Value => value;

        [SerializeField] private PublishVector2IntEvent publisher = default;
        [ValuePublisher("Value")] protected override UnityEvent<Vector2Int> Publisher => publisher ?? (publisher = new PublishVector2IntEvent());

        protected override Vector2Int Provide()
        {
            return Value;
        }
    }
}
