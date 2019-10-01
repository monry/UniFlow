using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    public abstract class ComparerBase<TValue, TOperator> : ConnectorBase
        where TOperator : Enum
    {
        [SerializeField] private TValue expect = default;
        [SerializeField] private TOperator @operator = default;

        [UsedImplicitly] public TValue Expect
        {
            get => expect;
            set => expect = value;
        }
        [UsedImplicitly] public TOperator Operator
        {
            get => @operator;
            set => @operator = value;
        }

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return Observable.ReturnUnit();
        }

        protected abstract bool Compare(TValue compareValue);

        private void OnDestroy()
        {
            Disposable.Dispose();
        }
    }
}
