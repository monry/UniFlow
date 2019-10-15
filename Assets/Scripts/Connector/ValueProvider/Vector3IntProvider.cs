using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3Int", (int) ConnectorType.ValueProviderVector3Int)]
    public class Vector3IntProvider : ProviderBase<Vector3Int, PublishVector3IntEvent, Vector3IntCollector>, IValueCombiner<Vector3Int>, IValueExtractor<Vector3Int>
    {
        private int? x;
        private int? y;
        private int? z;
        [ValueReceiver] private int X { get => x ?? Value.x; set => x = value; }
        [ValueReceiver] private int Y { get => y ?? Value.y; set => y = value; }
        [ValueReceiver] private int Z { get => z ?? Value.z; set => z = value; }

        [SerializeField] private PublishIntEvent publisherX = new PublishIntEvent();
        [SerializeField] private PublishIntEvent publisherY = new PublishIntEvent();
        [SerializeField] private PublishIntEvent publisherZ = new PublishIntEvent();

        [ValuePublisher("X")] private UnityEvent<int> PublisherX => publisherX;
        [ValuePublisher("Y")] private UnityEvent<int> PublisherY => publisherY;
        [ValuePublisher("Z")] private UnityEvent<int> PublisherZ => publisherZ;

        Vector3Int IValueCombiner<Vector3Int>.Combine()
        {
            return new Vector3Int(X, Y, Z);
        }

        void IValueExtractor<Vector3Int>.Extract(Vector3Int value)
        {
            PublisherX.Invoke(value.x);
            PublisherY.Invoke(value.y);
            PublisherZ.Invoke(value.z);
        }
    }
}
