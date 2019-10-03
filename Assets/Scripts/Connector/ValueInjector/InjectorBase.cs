using System;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.ValueInjector
{
    public abstract class InjectorBase : ConnectorBase
    {
        public abstract GameObject BaseGameObject { get; set; }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Inject();
            return Observable.ReturnUnit();
        }

        protected abstract void Inject();
    }
}
