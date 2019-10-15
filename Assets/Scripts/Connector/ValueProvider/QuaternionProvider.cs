using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Quaternion", (int) ConnectorType.ValueProviderQuaternion)]
    public class QuaternionProvider : ProviderBase<Quaternion, PublishQuaternionEvent, QuaternionCollector>, IValueCombiner<Quaternion>, IValueExtractor<Quaternion>
    {
        private float? x;
        private float? y;
        private float? z;
        private float? w;
        private Vector3? eulerAngle;
        [ValueReceiver] private float X { get => x ?? Value.x; set => x = value; }
        [ValueReceiver] private float Y { get => y ?? Value.y; set => y = value; }
        [ValueReceiver] private float Z { get => z ?? Value.z; set => z = value; }
        [ValueReceiver] private float W { get => w ?? Value.w; set => w = value; }
        // Should split class to QuaternionEulerProvider?
        [ValueReceiver] private Vector3 EulerAngle { get => eulerAngle ?? Value.eulerAngles; set => eulerAngle = value; }

        [SerializeField] private PublishFloatEvent publisherX = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherY = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherZ = new PublishFloatEvent();
        [SerializeField] private PublishFloatEvent publisherW = new PublishFloatEvent();
        [SerializeField] private PublishVector3Event publisherEulerAngle = new PublishVector3Event();

        [ValuePublisher("X")] private UnityEvent<float> PublisherX => publisherX;
        [ValuePublisher("Y")] private UnityEvent<float> PublisherY => publisherY;
        [ValuePublisher("Z")] private UnityEvent<float> PublisherZ => publisherZ;
        [ValuePublisher("W")] private UnityEvent<float> PublisherW => publisherW;
        // Should split class to QuaternionEulerProvider?
        [ValuePublisher("EulerAngle")] private UnityEvent<Vector3> PublisherEulerAngle => publisherEulerAngle;

        Quaternion IValueCombiner<Quaternion>.Combine()
        {
            return eulerAngle != null ? Quaternion.Euler(EulerAngle) : new Quaternion(X, Y, Z, W);
        }

        void IValueExtractor<Quaternion>.Extract(Quaternion value)
        {
            PublisherEulerAngle.Invoke(value.eulerAngles);
            PublisherX.Invoke(value.x);
            PublisherY.Invoke(value.y);
            PublisherZ.Invoke(value.z);
            PublisherW.Invoke(value.w);
        }
    }
}
