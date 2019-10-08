using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2Int", (int) ConnectorType.ValueProviderVector2Int)]
    public class Vector2IntProvider : ValueProviderBase<Vector2Int, PublishVector2IntEvent>
    {
        protected override Vector2Int Provide()
        {
            return Value;
        }
    }
}
