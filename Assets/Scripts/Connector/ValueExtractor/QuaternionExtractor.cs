using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueExtractor
{
    [AddComponentMenu("UniFlow/ValueExtractor/Quaternion", (int) ConnectorType.ValueExtractorQuaternion)]
    public class QuaternionExtractor : ConnectorBase
    {
        [SerializeField] private PublishFloatEvent publisherX = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherY = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherZ = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherW = new PublishFloatEvent();

        [ValueReceiver] private Quaternion Value { get; set; }
        [ValuePublisher("X")] private UnityEvent<float> PublisherX => publisherX;
        [ValuePublisher("Y")] private UnityEvent<float> PublisherY => publisherY;
        [ValuePublisher("Z")] private UnityEvent<float> PublisherZ => publisherZ;
        [ValuePublisher("W")] private UnityEvent<float> PublisherW => publisherW;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            PublisherX.Invoke(Value.x);
            PublisherY.Invoke(Value.y);
            PublisherZ.Invoke(Value.z);
            PublisherW.Invoke(Value.w);
            return Observable.ReturnUnit();
        }
    }
}
