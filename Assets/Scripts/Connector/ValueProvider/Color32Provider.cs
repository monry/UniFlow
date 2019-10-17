using System.Collections.Generic;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color32", (int) ConnectorType.ValueProviderColor32)]
    public class Color32Provider : ProviderBase<Color32>, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private ByteCollector rCollector = new ByteCollector();
        [SerializeField] private ByteCollector gCollector = new ByteCollector();
        [SerializeField] private ByteCollector bCollector = new ByteCollector();
        [SerializeField] private ByteCollector aCollector = new ByteCollector();

        private ByteCollector RCollector => rCollector;
        private ByteCollector GCollector => gCollector;
        private ByteCollector BCollector => bCollector;
        private ByteCollector ACollector => aCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotation<byte>.Create(RCollector, x => Value = new Color32(x, Value.g, Value.b, Value.a), "R"),
                CollectableMessageAnnotation<byte>.Create(GCollector, x => Value = new Color32(Value.r, x, Value.b, Value.a), "G"),
                CollectableMessageAnnotation<byte>.Create(BCollector, x => Value = new Color32(Value.r, Value.g, x, Value.a), "B"),
                CollectableMessageAnnotation<byte>.Create(ACollector, x => Value = new Color32(Value.r, Value.g, Value.b, x), "A"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotation<Color32>.Create(() => Value),
            };
    }
}
