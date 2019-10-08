using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3", (int) ConnectorType.ValueProviderVector3)]
    public class Vector3Provider : ValueProviderBase<Vector3, PublishVector3Event>
    {
        protected override Vector3 Provide()
        {
            return Value;
        }
    }
}
