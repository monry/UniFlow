using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Byte", (int) ConnectorType.ValueProviderByte)]
    public class ByteProvider : ValueProviderBase<byte, PublishByteEvent>
    {
        protected override byte Provide()
        {
            return Value;
        }
    }
}
