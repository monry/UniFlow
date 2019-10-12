using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color32", (int) ConnectorType.ValueProviderColor32)]
    public class Color32Provider : ProviderBase<Color32, PublishColor32Event>, IValueCombiner<Color32>, IValueExtractor<Color32>
    {
        private byte? r;
        private byte? g;
        private byte? b;
        private byte? a;
        [ValueReceiver] private byte R { get => r ?? Value.r; set => r = value; }
        [ValueReceiver] private byte G { get => g ?? Value.g; set => g = value; }
        [ValueReceiver] private byte B { get => b ?? Value.b; set => b = value; }
        [ValueReceiver] private byte A { get => a ?? Value.a; set => a = value; }

        [SerializeField] private PublishByteEvent publisherR = new PublishByteEvent();
        [SerializeField] private PublishByteEvent publisherG = new PublishByteEvent();
        [SerializeField] private PublishByteEvent publisherB = new PublishByteEvent();
        [SerializeField] private PublishByteEvent publisherA = new PublishByteEvent();

        [ValuePublisher("R")] private UnityEvent<byte> PublisherR => publisherR;
        [ValuePublisher("G")] private UnityEvent<byte> PublisherG => publisherG;
        [ValuePublisher("B")] private UnityEvent<byte> PublisherB => publisherB;
        [ValuePublisher("A")] private UnityEvent<byte> PublisherA => publisherA;

        Color32 IValueCombiner<Color32>.Combine()
        {
            return new Color32(R, G, B, A);
        }

        void IValueExtractor<Color32>.Extract(Color32 value)
        {
            PublisherR.Invoke(value.r);
            PublisherG.Invoke(value.g);
            PublisherB.Invoke(value.b);
            PublisherA.Invoke(value.a);
        }
    }
}
