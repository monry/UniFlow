using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3Int", (int) ConnectorType.ValueProviderVector3Int)]
    public class Vector3IntProvider : ValueProviderBase<Vector3Int, PublishVector3IntEvent>
    {
        protected override Vector3Int Provide()
        {
            return Value;
        }
    }
}
