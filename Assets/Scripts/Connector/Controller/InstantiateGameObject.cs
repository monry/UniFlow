using System;
using UniFlow.Utility;
using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/InstantiateObject", (int) ConnectorType.InstantiateGameObject)]
    public class InstantiateGameObject : ConnectorBase
    {
        [SerializeField] private GameObject source = default;
        [SerializeField] private Transform parent = default;

        [SerializeField] private PublishGameObjectEvent publisher = new PublishGameObjectEvent();

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

        [ValuePublisher("Instantiated")] private PublishGameObjectEvent Publisher => publisher;

        public override IObservable<Message> OnConnectAsObservable()
        {
            var go = Instantiate(Source, Parent);
            Publisher.Invoke(go);
            return ObservableFactory.ReturnMessage(this);
        }
    }
}
