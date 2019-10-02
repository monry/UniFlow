using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Quaternion", (int) ConnectorType.ValueProviderQuaternion)]
    public class QuaternionProvider : ProviderBase<Quaternion, PublishQuaternionEvent>
    {
        [SerializeField] private Quaternion value = default;
        private Quaternion Value => value;

        protected override Quaternion Provide()
        {
            return Value;
        }
    }
}
