using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2Int", (int) ConnectorType.ValueProviderVector2Int)]
    public class Vector2Int : Base<UnityEngine.Vector2Int>
    {
        [SerializeField] private UnityEngine.Vector2Int value = default;
        private UnityEngine.Vector2Int Value => value;

        [SerializeField] private PublishVector2IntEvent publisher = default;
        [ValuePublisher("Value")]
        private PublishVector2IntEvent Publisher => publisher ?? (publisher = new PublishVector2IntEvent());

        protected override UnityEngine.Vector2Int Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
