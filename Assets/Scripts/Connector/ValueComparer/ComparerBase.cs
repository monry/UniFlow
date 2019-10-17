using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    public abstract class ComparerBase<TValue, TCollector> : ConnectorBase, IMessageCollectable, IMessageComposable
        where TCollector : ValueCollectorBase<TValue>, new()
    {
        private const string MessageParameterKey = "Result";

        [SerializeField] private TValue expect = default;

        protected TValue Expect
        {
            get => expect;
            private set => expect = value;
        }
        private TValue Actual { get; set; }

        [SerializeField] private TCollector expectCollector = new TCollector();
        [SerializeField] private TCollector actualCollector = new TCollector();

        private TCollector ExpectCollector => expectCollector;
        private TCollector ActualCollector => actualCollector;

        private bool Result { get; set; }

        public override IObservable<Message> OnConnectAsObservable()
        {
            Result = Compare(Actual);
            return Result ? ObservableFactory.ReturnMessage(this, MessageParameterKey, true) : ObservableFactory.EmptyMessage();
        }

        protected abstract bool Compare(TValue actual);

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotation<TValue>.Create(ExpectCollector, x => Expect = x, nameof(Expect)),
                CollectableMessageAnnotation<TValue>.Create(ActualCollector, x => Actual = x, nameof(Actual)),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotation<bool>.Create(() => Result, MessageParameterKey),
            };
    }

    public abstract class ComparerBase<TValue, TOperator, TCollector> : ComparerBase<TValue, TCollector>
        where TOperator : Enum
        where TCollector : ValueCollectorBase<TValue>, new()
    {
        [SerializeField] private TOperator @operator = default;

        [UsedImplicitly] public TOperator Operator
        {
            get => @operator;
            set => @operator = value;
        }
    }
}
