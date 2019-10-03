using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.ValueInjector
{
    public abstract class InjectorBase : ConnectorBase
    {
        [SerializeField] private GameObject baseGameObject = default;

        [ValueReceiver] public GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Inject();
            return Observable.ReturnUnit();
        }

        protected abstract void Inject();
    }
}
