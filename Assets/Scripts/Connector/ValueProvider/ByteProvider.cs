using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Byte", (int) ConnectorType.ValueProviderByte)]
    public class ByteProvider : ProviderBase<byte>
    {
        [SerializeField] private byte value = default;
        private byte Value => value;

        [SerializeField] private PublishByteEvent publisher = default;
        [ValuePublisher("Value")] protected override UnityEvent<byte> Publisher => publisher ?? (publisher = new PublishByteEvent());

        protected override byte Provide()
        {
            return Value;
        }
    }
}
