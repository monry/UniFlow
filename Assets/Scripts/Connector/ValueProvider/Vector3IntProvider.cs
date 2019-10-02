using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3Int", (int) ConnectorType.ValueProviderVector3Int)]
    public class Vector3IntProvider : ProviderBase<Vector3Int, PublishVector3IntEvent>
    {
        [SerializeField] private Vector3Int value = default;
        private Vector3Int Value => value;

        protected override Vector3Int Provide()
        {
            return Value;
        }
    }
}
