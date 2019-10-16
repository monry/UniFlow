using System.Collections.Generic;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3", (int) ConnectorType.ValueProviderVector3)]
    public class Vector3Provider : ProviderBase<Vector3>, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private FloatCollector xCollector = default;
        [SerializeField] private FloatCollector yCollector = default;
        [SerializeField] private FloatCollector zCollector = default;

        private FloatCollector XCollector => xCollector;
        private FloatCollector YCollector => yCollector;
        private FloatCollector ZCollector => zCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                new CollectableMessageAnnotation<float>(XCollector, v => Value = new Vector3(v, Value.y, Value.z), "X"),
                new CollectableMessageAnnotation<float>(YCollector, v => Value = new Vector3(Value.x, v, Value.z), "Y"),
                new CollectableMessageAnnotation<float>(ZCollector, v => Value = new Vector3(Value.x, Value.y, v), "Z"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new IComposableMessageAnnotation[]
            {
                new ComposableMessageAnnotation<Vector3>(() => Value),
            };
    }
}
