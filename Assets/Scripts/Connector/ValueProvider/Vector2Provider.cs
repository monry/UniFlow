using System.Collections.Generic;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2", (int) ConnectorType.ValueProviderVector2)]
    public class Vector2Provider : ProviderBase<Vector2>, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private FloatCollector xCollector = default;
        [SerializeField] private FloatCollector yCollector = default;

        private FloatCollector XCollector => xCollector;
        private FloatCollector YCollector => yCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                new CollectableMessageAnnotation<float>(XCollector, v => Value = new Vector2(v, Value.y), "X"),
                new CollectableMessageAnnotation<float>(YCollector, v => Value = new Vector2(Value.x, v), "Y"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new IComposableMessageAnnotation[]
            {
                new ComposableMessageAnnotation<Vector2>(() => Value),
            };
    }
}
