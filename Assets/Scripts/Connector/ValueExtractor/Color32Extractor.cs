using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueExtractor
{
    [AddComponentMenu("UniFlow/ValueExtractor/Color32", (int) ConnectorType.ValueExtractorColor32)]
    public class Color32Extractor : ConnectorBase
    {
        [SerializeField] private PublishByteEvent publisherR = new PublishByteEvent();
        [SerializeField] private PublishByteEvent publisherG = new PublishByteEvent();
        [SerializeField] private PublishByteEvent publisherB = new PublishByteEvent();
        [SerializeField] private PublishByteEvent publisherA = new PublishByteEvent();

        [ValueReceiver] private Color32 Value { get; set; }
        [ValuePublisher("R")] private UnityEvent<byte> PublisherR => publisherR;
        [ValuePublisher("G")] private UnityEvent<byte> PublisherG => publisherG;
        [ValuePublisher("B")] private UnityEvent<byte> PublisherB => publisherB;
        [ValuePublisher("A")] private UnityEvent<byte> PublisherA => publisherA;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            PublisherR.Invoke(Value.r);
            PublisherG.Invoke(Value.g);
            PublisherB.Invoke(Value.b);
            PublisherA.Invoke(Value.a);
            return Observable.ReturnUnit();
        }
    }
}
