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
                new CollectableMessageAnnotation<float>(XCollector, v => Value = new Vector4(v, Value.y, Value.z, Value.w), "X"),
                new CollectableMessageAnnotation<float>(YCollector, v => Value = new Vector4(Value.x, v, Value.z, Value.w), "Y"),
                new CollectableMessageAnnotation<float>(ZCollector, v => Value = new Vector4(Value.x, Value.y, v, Value.w), "Z"),
                new CollectableMessageAnnotation<float>(WCollector, v => Value = new Vector4(Value.x, Value.y, Value.z, v), "W"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new IComposableMessageAnnotation[]
            {
                new ComposableMessageAnnotation<Vector4>(() => Value),
            };
    }
}
