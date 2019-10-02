using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2Int", (int) ConnectorType.ValueProviderVector2Int)]
    public class Vector2IntProvider : ProviderBase<Vector2Int, PublishVector2IntEvent>
    {
        [SerializeField] private Vector2Int value = default;
        private Vector2Int Value => value;

        protected override Vector2Int Provide()
        {
            return Value;
        }
    }
}
