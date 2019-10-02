using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector4", (int) ConnectorType.ValueProviderVector4)]
    public class Vector4Provider : ProviderBase<Vector4, PublishVector4Event>
    {
        [SerializeField] private Vector4 value = default;
        private Vector4 Value => value;

        protected override Vector4 Provide()
        {
            return Value;
        }
    }
}
