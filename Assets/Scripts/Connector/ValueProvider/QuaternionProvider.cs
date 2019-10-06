using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Quaternion", (int) ConnectorType.ValueProviderQuaternion)]
    public class QuaternionProvider : ValueProviderBase<Quaternion, PublishQuaternionEvent>
    {
        protected override Quaternion Provide()
        {
            return Value;
        }
    }
}
