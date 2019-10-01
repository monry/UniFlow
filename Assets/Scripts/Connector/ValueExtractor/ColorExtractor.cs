using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueExtractor
{
    [AddComponentMenu("UniFlow/ValueExtractor/Color", (int) ConnectorType.ValueExtractorColor)]
    public class ColorExtractor : ConnectorBase
    {
        [SerializeField] private PublishFloatEvent publisherR = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherG = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherB = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherA = new PublishFloatEvent();

        [ValueReceiver] private Color Value { get; set; }
        [ValuePublisher("R")] private UnityEvent<float> PublisherR => publisherR;
        [ValuePublisher("G")] private UnityEvent<float> PublisherG => publisherG;
        [ValuePublisher("B")] private UnityEvent<float> PublisherB => publisherB;
        [ValuePublisher("A")] private UnityEvent<float> PublisherA => publisherA;

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
