using System;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    public abstract class ComparerBase<TValue> : ConnectorBase
    {
        [SerializeField] private TValue expect = default;

        [ValueReceiver] public TValue Expect
        {
            get => expect;
            set => expect = value;
        }
        [ValueReceiver] public TValue Actual { get; set; }

        [SerializeField] private PublishBoolEvent publishResult = new PublishBoolEvent();
        [ValuePublisher("Result")] public PublishBoolEvent PublishResult => publishResult;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            var result = Compare(Actual);
            PublishResult.Invoke(result);
            return result ? Observable.ReturnUnit() : Observable.Empty<Unit>();
        }

        protected abstract bool Compare(TValue actual);
    }

    public abstract class ComparerBase<TValue, TOperator> : ComparerBase<TValue>
        where TOperator : Enum
    {
        [SerializeField] private TOperator @operator = default;

        [UsedImplicitly] public TOperator Operator
        {
            get => @operator;
            set => @operator = value;
        }
    }
}
