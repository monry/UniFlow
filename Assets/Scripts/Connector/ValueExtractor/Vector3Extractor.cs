using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueExtractor
{
    [AddComponentMenu("UniFlow/ValueExtractor/Vector3", (int) ConnectorType.ValueExtractorVector3)]
    public class Vector3Extractor : ConnectorBase
    {
        [SerializeField] private PublishFloatEvent publisherX = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherY = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherZ = new PublishFloatEvent();

        [ValueReceiver] private Vector3 Value { get; set; }
        [ValuePublisher("X")] private UnityEvent<float> PublisherX => publisherX;
        [ValuePublisher("Y")] private UnityEvent<float> PublisherY => publisherY;
        [ValuePublisher("Z")] private UnityEvent<float> PublisherZ => publisherZ;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            PublisherX.Invoke(Value.x);
            PublisherY.Invoke(Value.y);
            PublisherZ.Invoke(Value.z);
            return Observable.ReturnUnit();
        }
    }
}
