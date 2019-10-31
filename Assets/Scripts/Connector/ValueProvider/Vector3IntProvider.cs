using System.Collections.Generic;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3IntProvider", (int) ConnectorType.ValueProviderVector3Int)]
    public class Vector3IntProvider : ProviderBase<Vector3Int>, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private IntCollector xCollector = new IntCollector();
        [SerializeField] private IntCollector yCollector = new IntCollector();
        [SerializeField] private IntCollector zCollector = new IntCollector();

        private IntCollector XCollector => xCollector;
        private IntCollector YCollector => yCollector;
        private IntCollector ZCollector => zCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(XCollector, v => Value = new Vector3Int(v, Value.y, Value.z), "X"),
                CollectableMessageAnnotationFactory.Create(YCollector, v => Value = new Vector3Int(Value.x, v, Value.z), "Y"),
                CollectableMessageAnnotationFactory.Create(ZCollector, v => Value = new Vector3Int(Value.x, Value.y, v), "Z"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create(() => Value),
            };
    }
}
