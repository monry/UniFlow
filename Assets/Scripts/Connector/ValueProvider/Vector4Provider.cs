using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector4", (int) ConnectorType.ValueProviderVector4)]
    public class Vector4Provider : ProviderBase<Vector4, PublishVector4Event>, IValueCombiner<Vector4>, IValueExtractor<Vector4>
    {
        private float? x;
        private float? y;
        private float? z;
        private float? w;
        [ValueReceiver] private float X { get => x ?? Value.x; set => x = value; }
        [ValueReceiver] private float Y { get => y ?? Value.y; set => y = value; }
        [ValueReceiver] private float Z { get => z ?? Value.z; set => z = value; }
        [ValueReceiver] private float W { get => w ?? Value.w; set => w = value; }

        [SerializeField] private PublishFloatEvent publisherX = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherY = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherZ = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherW = new PublishFloatEvent();

        [ValuePublisher("X")] private UnityEvent<float> PublisherX => publisherX;
        [ValuePublisher("Y")] private UnityEvent<float> PublisherY => publisherY;
        [ValuePublisher("Z")] private UnityEvent<float> PublisherZ => publisherZ;
        [ValuePublisher("W")] private UnityEvent<float> PublisherW => publisherW;

        Vector4 IValueCombiner<Vector4>.Combine()
        {
            return new Vector4(X, Y, Z, W);
        }

        void IValueExtractor<Vector4>.Extract(Vector4 value)
        {
            PublisherX.Invoke(value.x);
            PublisherY.Invoke(value.y);
            PublisherZ.Invoke(value.z);
            PublisherW.Invoke(value.w);
        }
    }
}
