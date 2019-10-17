using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.Expression
{
    [AddComponentMenu("UniFlow/Expression/Xor", (int) ConnectorType.Xor)]
    public class Xor : ConnectorBase, IMessageCollectable
    {
        private bool Left { get; set; }
        private bool Right { get; set; }

        [SerializeField] private BoolCollector leftCollector = new BoolCollector();
        [SerializeField] private BoolCollector rightCollector = new BoolCollector();

        private BoolCollector LeftCollector => leftCollector;
        private BoolCollector RightCollector => rightCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return Left ^ Right ? ObservableFactory.ReturnMessage(this) : ObservableFactory.EmptyMessage();
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotation<bool>.Create(LeftCollector, x => Left = x, nameof(Left)),
                CollectableMessageAnnotation<bool>.Create(RightCollector, x => Right = x, nameof(Right)),
            };
    }
}
