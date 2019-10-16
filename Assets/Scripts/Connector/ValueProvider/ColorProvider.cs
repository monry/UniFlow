using System.Collections.Generic;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color", (int) ConnectorType.ValueProviderColor)]
    public class ColorProvider : ProviderBase<Color>, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private FloatCollector rCollector = default;
        [SerializeField] private FloatCollector gCollector = default;
        [SerializeField] private FloatCollector bCollector = default;
        [SerializeField] private FloatCollector aCollector = default;

        private FloatCollector RCollector => rCollector;
        private FloatCollector GCollector => gCollector;
        private FloatCollector BCollector => bCollector;
        private FloatCollector ACollector => aCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                new CollectableMessageAnnotation<float>(RCollector, x => Value = new Color(x, Value.g, Value.b, Value.a), "R"),
                new CollectableMessageAnnotation<float>(GCollector, x => Value = new Color(Value.r, x, Value.b, Value.a), "G"),
                new CollectableMessageAnnotation<float>(BCollector, x => Value = new Color(Value.r, Value.g, x, Value.a), "B"),
                new CollectableMessageAnnotation<float>(ACollector, x => Value = new Color(Value.r, Value.g, Value.b, x), "A"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                new ComposableMessageAnnotation<Color>(() => Value),
            };
    }
}
