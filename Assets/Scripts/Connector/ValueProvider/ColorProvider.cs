using System.Collections.Generic;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color", (int) ConnectorType.ValueProviderColor)]
    public class ColorProvider : ProviderBase<Color>, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private FloatCollector rCollector = new FloatCollector();
        [SerializeField] private FloatCollector gCollector = new FloatCollector();
        [SerializeField] private FloatCollector bCollector = new FloatCollector();
        [SerializeField] private FloatCollector aCollector = new FloatCollector();

        private FloatCollector RCollector => rCollector;
        private FloatCollector GCollector => gCollector;
        private FloatCollector BCollector => bCollector;
        private FloatCollector ACollector => aCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotation<float>.Create(RCollector, x => Value = new Color(x, Value.g, Value.b, Value.a), "R"),
                CollectableMessageAnnotation<float>.Create(GCollector, x => Value = new Color(Value.r, x, Value.b, Value.a), "G"),
                CollectableMessageAnnotation<float>.Create(BCollector, x => Value = new Color(Value.r, Value.g, x, Value.a), "B"),
                CollectableMessageAnnotation<float>.Create(ACollector, x => Value = new Color(Value.r, Value.g, Value.b, x), "A"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotation<Color>.Create(() => Value),
            };
    }
}
