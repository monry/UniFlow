using System.Collections.Generic;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3Int", (int) ConnectorType.ValueProviderVector3Int)]
    public class Vector3IntProvider : ProviderBase<Vector3Int>, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private IntCollector xCollector = default;
        [SerializeField] private IntCollector yCollector = default;
        [SerializeField] private IntCollector zCollector = default;

        private IntCollector XCollector => xCollector;
        private IntCollector YCollector => yCollector;
        private IntCollector ZCollector => zCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                new CollectableMessageAnnotation<int>(XCollector, v => Value = new Vector3Int(v, Value.y, Value.z), "X"),
                new CollectableMessageAnnotation<int>(YCollector, v => Value = new Vector3Int(Value.x, v, Value.z), "Y"),
                new CollectableMessageAnnotation<int>(ZCollector, v => Value = new Vector3Int(Value.x, Value.y, v), "Z"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new IComposableMessageAnnotation[]
            {
                new ComposableMessageAnnotation<Vector3Int>(() => Value),
            };
    }
}
