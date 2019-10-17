using System.Collections.Generic;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector4", (int) ConnectorType.ValueProviderVector4)]
    public class Vector4Provider : ProviderBase<Vector4>, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private FloatCollector xCollector = default;
        [SerializeField] private FloatCollector yCollector = default;
        [SerializeField] private FloatCollector zCollector = default;
        [SerializeField] private FloatCollector wCollector = default;

        private FloatCollector XCollector => xCollector;
        private FloatCollector YCollector => yCollector;
        private FloatCollector ZCollector => zCollector;
        private FloatCollector WCollector => wCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                CollectableMessageAnnotation<float>.Create(XCollector, v => Value = new Vector4(v, Value.y, Value.z, Value.w), "X"),
                CollectableMessageAnnotation<float>.Create(YCollector, v => Value = new Vector4(Value.x, v, Value.z, Value.w), "Y"),
                CollectableMessageAnnotation<float>.Create(ZCollector, v => Value = new Vector4(Value.x, Value.y, v, Value.w), "Z"),
                CollectableMessageAnnotation<float>.Create(WCollector, v => Value = new Vector4(Value.x, Value.y, Value.z, v), "W"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new IComposableMessageAnnotation[]
            {
                ComposableMessageAnnotation<Vector4>.Create(() => Value),
            };
    }
}
