using System;
using System.Collections.Generic;
using System.Linq;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.Expression
{
    [AddComponentMenu("UniFlow/Expression/And", (int) ConnectorType.And)]
    public class And : ConnectorBase, IMessageCollectable
    {
        private IList<bool> Conditions { get; } = new List<bool>();

        private bool Value
        {
            get => Conditions.All(x => x);
            set => Conditions.Add(value);
        }

        [SerializeField] private BoolCollector valueCollector = new BoolCollector();
        private BoolCollector ValueCollector => valueCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return Value ? ObservableFactory.ReturnMessage(this) : ObservableFactory.EmptyMessage();
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotation<bool>.Create(ValueCollector, x => Value = x, nameof(Value)),
            };
    }
}
