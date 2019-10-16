using System.Collections.Generic;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2Int", (int) ConnectorType.ValueProviderVector2Int)]
    public class Vector2IntProvider : ProviderBase<Vector2Int>, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private IntCollector xCollector = default;
        [SerializeField] private IntCollector yCollector = default;

        private IntCollector XCollector => xCollector;
        private IntCollector YCollector => yCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                new CollectableMessageAnnotation<int>(XCollector, v => Value = new Vector2Int(v, Value.y), "X"),
                new CollectableMessageAnnotation<int>(YCollector, v => Value = new Vector2Int(Value.x, v), "Y"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new IComposableMessageAnnotation[]
            {
                new ComposableMessageAnnotation<Vector2Int>(() => Value),
            };
    }
}
