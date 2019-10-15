using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color", (int) ConnectorType.ValueProviderColor)]
    public class ColorProvider : ProviderBase<Color, PublishColorEvent, ColorCollector>, IValueCombiner<Color>, IValueExtractor<Color>
    {
        private float? r;
        private float? g;
        private float? b;
        private float? a;
        [ValueReceiver] private float R { get => r ?? Value.r; set => r = value; }
        [ValueReceiver] private float G { get => g ?? Value.g; set => g = value; }
        [ValueReceiver] private float B { get => b ?? Value.b; set => b = value; }
        [ValueReceiver] private float A { get => a ?? Value.a; set => a = value; }

        [SerializeField] private PublishFloatEvent publisherR = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherG = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherB = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherA = new PublishFloatEvent();

        [ValuePublisher("R")] private UnityEvent<float> PublisherR => publisherR;
        [ValuePublisher("G")] private UnityEvent<float> PublisherG => publisherG;
        [ValuePublisher("B")] private UnityEvent<float> PublisherB => publisherB;
        [ValuePublisher("A")] private UnityEvent<float> PublisherA => publisherA;

        Color IValueCombiner<Color>.Combine()
        {
            return new Color(R, G, B, A);
        }

        void IValueExtractor<Color>.Extract(Color value)
        {
            PublisherR.Invoke(value.r);
            PublisherG.Invoke(value.g);
            PublisherB.Invoke(value.b);
            PublisherA.Invoke(value.a);
        }
    }
}
