using System.Collections.Generic;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3Provider", (int) ConnectorType.ValueProviderVector3)]
    public class Vector3Provider : ProviderBase<Vector3>, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private FloatCollector xCollector = new FloatCollector();
        [SerializeField] private FloatCollector yCollector = new FloatCollector();
        [SerializeField] private FloatCollector zCollector = new FloatCollector();

        private FloatCollector XCollector => xCollector;
        private FloatCollector YCollector => yCollector;
        private FloatCollector ZCollector => zCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(XCollector, v => Value = new Vector3(v, Value.y, Value.z), "X"),
                CollectableMessageAnnotationFactory.Create(YCollector, v => Value = new Vector3(Value.x, v, Value.z), "Y"),
                CollectableMessageAnnotationFactory.Create(ZCollector, v => Value = new Vector3(Value.x, Value.y, v), "Z"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create(() => Value),
            };
    }
}
