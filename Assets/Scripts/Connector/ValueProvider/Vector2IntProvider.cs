using System.Collections.Generic;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2Int", (int) ConnectorType.ValueProviderVector2Int)]
    public class Vector2IntProvider : ProviderBase<Vector2Int>, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private IntCollector xCollector = new IntCollector();
        [SerializeField] private IntCollector yCollector = new IntCollector();

        private IntCollector XCollector => xCollector;
        private IntCollector YCollector => yCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(XCollector, v => Value = new Vector2Int(v, Value.y), "X"),
                CollectableMessageAnnotationFactory.Create(YCollector, v => Value = new Vector2Int(Value.x, v), "Y"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create(() => Value),
            };
    }
}
