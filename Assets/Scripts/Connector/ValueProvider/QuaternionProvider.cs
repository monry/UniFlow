using System.Collections.Generic;
using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Quaternion", (int) ConnectorType.ValueProviderQuaternion)]
    public class QuaternionProvider : ProviderBase<Quaternion>, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private FloatCollector xCollector = default;
        [SerializeField] private FloatCollector yCollector = default;
        [SerializeField] private FloatCollector zCollector = default;
        [SerializeField] private FloatCollector wCollector = default;
        [SerializeField] private Vector3Collector eulerAngleCollector = default;

        private FloatCollector XCollector => xCollector;
        private FloatCollector YCollector => yCollector;
        private FloatCollector ZCollector => zCollector;
        private FloatCollector WCollector => wCollector;
        private Vector3Collector EulerAngleCollector => eulerAngleCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                new CollectableMessageAnnotation<float>(XCollector, v => Value = new Quaternion(v, Value.y, Value.z, Value.w), "X"),
                new CollectableMessageAnnotation<float>(YCollector, v => Value = new Quaternion(Value.x, v, Value.z, Value.w), "Y"),
                new CollectableMessageAnnotation<float>(ZCollector, v => Value = new Quaternion(Value.x, Value.y, v, Value.w), "Z"),
                new CollectableMessageAnnotation<float>(WCollector, v => Value = new Quaternion(Value.x, Value.y, Value.z, v), "W"),
                new CollectableMessageAnnotation<Vector3>(EulerAngleCollector, v => Value = Quaternion.Euler(v), "EulerAngle"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new IComposableMessageAnnotation[]
            {
                new ComposableMessageAnnotation<Quaternion>(() => Value),
                new ComposableMessageAnnotation<Vector3>(() => Value.eulerAngles),
            };
    }
}
