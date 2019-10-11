using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3", (int) ConnectorType.ValueProviderVector3)]
    public class Vector3Provider : ProviderBase<Vector3, PublishVector3Event>, IValueCombiner<Vector3>, IValueExtractor<Vector3>
    {
        private float? x;
        private float? y;
        private float? z;
        [ValueReceiver] private float X { get => x ?? Value.x; set => x = value; }
        [ValueReceiver] private float Y { get => y ?? Value.y; set => y = value; }
        [ValueReceiver] private float Z { get => z ?? Value.z; set => z = value; }

        [SerializeField] private PublishFloatEvent publisherX = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherY = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherZ = new PublishFloatEvent();

        [ValuePublisher("X")] private UnityEvent<float> PublisherX => publisherX;
        [ValuePublisher("Y")] private UnityEvent<float> PublisherY => publisherY;
        [ValuePublisher("Z")] private UnityEvent<float> PublisherZ => publisherZ;

        Vector3 IValueCombiner<Vector3>.Combine()
        {
            return new Vector3(X, Y, Z);
        }

        void IValueExtractor<Vector3>.Extract(Vector3 value)
        {
            PublisherX.Invoke(value.x);
            PublisherY.Invoke(value.y);
            PublisherZ.Invoke(value.z);
        }
    }
}
