using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3", (int) ConnectorType.ValueProviderVector3)]
    public class Vector3Provider : ProviderBase<Vector3, PublishVector3Event>
    {
        [SerializeField] private Vector3 value = default;
        private Vector3 Value => value;

        protected override Vector3 Provide()
        {
            return Value;
        }
    }
}
