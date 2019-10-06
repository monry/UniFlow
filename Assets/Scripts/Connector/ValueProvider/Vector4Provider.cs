using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector4", (int) ConnectorType.ValueProviderVector4)]
    public class Vector4Provider : ValueProviderBase<Vector4, PublishVector4Event>
    {
        protected override Vector4 Provide()
        {
            return Value;
        }
    }
}
