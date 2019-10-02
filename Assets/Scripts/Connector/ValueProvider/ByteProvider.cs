using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Byte", (int) ConnectorType.ValueProviderByte)]
    public class ByteProvider : ProviderBase<byte, PublishByteEvent>
    {
        [SerializeField] private byte value = default;
        private byte Value => value;

        protected override byte Provide()
        {
            return Value;
        }
    }
}
