using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueExtractor
{
    [AddComponentMenu("UniFlow/ValueExtractor/Vector2", (int) ConnectorType.ValueExtractorVector2)]
    public class Vector2Extractor : ConnectorBase
    {
        [SerializeField] private PublishFloatEvent publisherX = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherY = new PublishFloatEvent();

        [ValueReceiver] private Vector2 Value { get; set; }
        [ValuePublisher("X")] private UnityEvent<float> PublisherX => publisherX;
        [ValuePublisher("Y")] private UnityEvent<float> PublisherY => publisherY;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            PublisherX.Invoke(Value.x);
            PublisherY.Invoke(Value.y);
            return Observable.ReturnUnit();
        }
    }
}
