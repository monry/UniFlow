using System.Collections.Generic;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color32", (int) ConnectorType.ValueProviderColor32)]
    public class Color32Provider : ProviderBase<Color32>, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private ByteCollector rCollector = default;
        [SerializeField] private ByteCollector gCollector = default;
        [SerializeField] private ByteCollector bCollector = default;
        [SerializeField] private ByteCollector aCollector = default;

        private ByteCollector RCollector => rCollector;
        private ByteCollector GCollector => gCollector;
        private ByteCollector BCollector => bCollector;
        private ByteCollector ACollector => aCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                new CollectableMessageAnnotation<byte>(RCollector, x => Value = new Color32(x, Value.g, Value.b, Value.a), "R"),
                new CollectableMessageAnnotation<byte>(GCollector, x => Value = new Color32(Value.r, x, Value.b, Value.a), "G"),
                new CollectableMessageAnnotation<byte>(BCollector, x => Value = new Color32(Value.r, Value.g, x, Value.a), "B"),
                new CollectableMessageAnnotation<byte>(ACollector, x => Value = new Color32(Value.r, Value.g, Value.b, x), "A"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                new ComposableMessageAnnotation<Color32>(() => Value),
            };
    }
}
