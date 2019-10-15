using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2", (int) ConnectorType.ValueProviderVector2)]
    public class Vector2Provider : ProviderBase<Vector2, PublishVector2Event>, IValueCombiner<Vector2>, IValueExtractor<Vector2>
    {
        private float? x;
        private float? y;
        [ValueReceiver] private float X { get => x ?? Value.x; set => x = value; }
        [ValueReceiver] private float Y { get => y ?? Value.y; set => y = value; }

        [SerializeField] private PublishFloatEvent publisherX = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherY = new PublishFloatEvent();

        [ValuePublisher("X")] private UnityEvent<float> PublisherX => publisherX;
        [ValuePublisher("Y")] private UnityEvent<float> PublisherY => publisherY;

        Vector2 IValueCombiner<Vector2>.Combine()
        {
            return new Vector2(X, Y);
        }

        void IValueExtractor<Vector2>.Extract(Vector2 value)
        {
            PublisherX.Invoke(value.x);
            PublisherY.Invoke(value.y);
        }
    }
}
