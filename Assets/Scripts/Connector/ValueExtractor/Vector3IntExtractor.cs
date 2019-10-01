using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueExtractor
{
    [AddComponentMenu("UniFlow/ValueExtractor/Vector3Int", (int) ConnectorType.ValueExtractorVector3Int)]
    public class Vector3IntExtractor : ConnectorBase
    {
        [SerializeField] private PublishIntEvent publisherX = new PublishIntEvent();
        [SerializeField] private PublishIntEvent publisherY = new PublishIntEvent();
        [SerializeField] private PublishIntEvent publisherZ = new PublishIntEvent();

        [ValueReceiver] private Vector3Int Value { get; set; }
        [ValuePublisher("X")] private UnityEvent<int> PublisherX => publisherX;
        [ValuePublisher("Y")] private UnityEvent<int> PublisherY => publisherY;
        [ValuePublisher("Z")] private UnityEvent<int> PublisherZ => publisherZ;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            PublisherX.Invoke(Value.x);
            PublisherY.Invoke(Value.y);
            PublisherZ.Invoke(Value.z);
            return Observable.ReturnUnit();
        }
    }
}
