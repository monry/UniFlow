using System.Collections.Generic;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector4Provider", (int) ConnectorType.ValueProviderVector4)]
    public class Vector4Provider : ProviderBase<Vector4>, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private FloatCollector xCollector = new FloatCollector();
        [SerializeField] private FloatCollector yCollector = new FloatCollector();
        [SerializeField] private FloatCollector zCollector = new FloatCollector();
        [SerializeField] private FloatCollector wCollector = new FloatCollector();

        private FloatCollector XCollector => xCollector;
        private FloatCollector YCollector => yCollector;
        private FloatCollector ZCollector => zCollector;
        private FloatCollector WCollector => wCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(XCollector, v => Value = new Vector4(v, Value.y, Value.z, Value.w), "X"),
                CollectableMessageAnnotationFactory.Create(YCollector, v => Value = new Vector4(Value.x, v, Value.z, Value.w), "Y"),
                CollectableMessageAnnotationFactory.Create(ZCollector, v => Value = new Vector4(Value.x, Value.y, v, Value.w), "Z"),
                CollectableMessageAnnotationFactory.Create(WCollector, v => Value = new Vector4(Value.x, Value.y, Value.z, v), "W"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create(() => Value),
            };
    }
}
