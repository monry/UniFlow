using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2Int", (int) ConnectorType.ValueProviderVector2Int)]
    public class Vector2IntProvider : ProviderBase<Vector2Int, PublishVector2IntEvent>, IValueCombiner<Vector2Int>, IValueExtractor<Vector2Int>
    {
        private int? x;
        private int? y;
        [ValueReceiver] private int X { get => x ?? Value.x; set => x = value; }
        [ValueReceiver] private int Y { get => y ?? Value.y; set => y = value; }

        [SerializeField] private PublishIntEvent publisherX = new PublishIntEvent();
        [SerializeField] private PublishIntEvent publisherY = new PublishIntEvent();

        [ValuePublisher("X")] private UnityEvent<int> PublisherX => publisherX;
        [ValuePublisher("Y")] private UnityEvent<int> PublisherY => publisherY;

        Vector2Int IValueCombiner<Vector2Int>.Combine()
        {
            return new Vector2Int(X, Y);
        }

        void IValueExtractor<Vector2Int>.Extract(Vector2Int value)
        {
            PublisherX.Invoke(value.x);
            PublisherY.Invoke(value.y);
        }
    }
}
