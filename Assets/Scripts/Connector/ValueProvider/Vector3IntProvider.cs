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
                CollectableMessageAnnotation<int>.Create(XCollector, v => Value = new Vector3Int(v, Value.y, Value.z), "X"),
                CollectableMessageAnnotation<int>.Create(YCollector, v => Value = new Vector3Int(Value.x, v, Value.z), "Y"),
                CollectableMessageAnnotation<int>.Create(ZCollector, v => Value = new Vector3Int(Value.x, Value.y, v), "Z"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new IComposableMessageAnnotation[]
            {
                ComposableMessageAnnotation<Vector3Int>.Create(() => Value),
            };
    }
}
