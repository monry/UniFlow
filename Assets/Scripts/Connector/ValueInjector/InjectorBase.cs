using System;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.ValueInjector
{
    public abstract class InjectorBase : ConnectorBase
    {
        public abstract GameObject BaseGameObject { get; set; }

        public override IObservable<Message> OnConnectAsObservable()
        {
            Inject();
            return ObservableFactory.ReturnMessage(this);
        }

        protected abstract void Inject();
    }
}
