using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueExtractor
{
    [AddComponentMenu("UniFlow/ValueExtractor/Vector2Int", (int) ConnectorType.ValueExtractorVector2Int)]
    public class Vector2IntExtractor : ConnectorBase
    {
        [SerializeField] private PublishIntEvent publisherX = new PublishIntEvent();
        [SerializeField] private PublishIntEvent publisherY = new PublishIntEvent();

        [ValueReceiver] private Vector2Int Value { get; set; }
        [ValuePublisher("X")] private UnityEvent<int> PublisherX => publisherX;
        [ValuePublisher("Y")] private UnityEvent<int> PublisherY => publisherY;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            PublisherX.Invoke(Value.x);
            PublisherY.Invoke(Value.y);
            return Observable.ReturnUnit();
        }
    }
}
