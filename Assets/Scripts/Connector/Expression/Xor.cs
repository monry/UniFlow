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

        [SerializeField] private BoolCollector leftCollector = default;
        [SerializeField] private BoolCollector rightCollector = default;

        private BoolCollector LeftCollector => leftCollector;
        private BoolCollector RightCollector => rightCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return Left ^ Right ? ObservableFactory.ReturnMessage(this) : ObservableFactory.EmptyMessage();
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                new CollectableMessageAnnotation<bool>(LeftCollector, x => Left = x, nameof(Left)),
                new CollectableMessageAnnotation<bool>(RightCollector, x => Right = x, nameof(Right)),
            };
    }
}
