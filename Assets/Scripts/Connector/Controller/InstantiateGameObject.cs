using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/InstantiateObject", (int) ConnectorType.InstantiateGameObject)]
    public class InstantiateGameObject : ConnectorBase
    {
        [SerializeField] private GameObject source = default;

        [SerializeField] private Transform parent = default;

        [SerializeField] private PublishGameObjectEvent publisher = new PublishGameObjectEvent();
        [ValuePublisher("Instantiated")] private PublishGameObjectEvent Publisher => publisher;

        [ValueReceiver] public GameObject Source
        {
            get => source == default ? gameObject : source;
            set => source = value;
        }
        [ValueReceiver] public Transform Parent
        {
            get => parent == default ? transform : parent;
            set => parent = value;
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            var go = Instantiate(Source, Parent);
            Publisher.Invoke(go);
            return Observable.ReturnUnit();
        }
    }
}
