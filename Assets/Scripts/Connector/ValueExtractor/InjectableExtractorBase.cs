using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.ValueExtractor
{
    public abstract class InjectableExtractorBase<T> : ConnectorBase, IInjectable<T>
    {
        [SerializeField] private T value = default;
        [ValueReceiver] public T Value
        {
            get => value;
            set => this.value = value;
        }

        private ISubject<Unit> OnExtractSubject { get; } = new AsyncSubject<Unit>();

        void IInjectable<T>.Inject(T v)
        {
            Value = v;
            OnExtractSubject.OnNext(Unit.Default);
            OnExtractSubject.OnCompleted();
        }

        IObservable<Unit> IInjectable<T>.WaitForInjectAsObservable()
        {
            return OnExtractSubject;
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return ((IInjectable<T>) this).WaitForInjectAsObservable().Do(_ => Extract());
        }

        protected abstract void Extract();
    }
}
