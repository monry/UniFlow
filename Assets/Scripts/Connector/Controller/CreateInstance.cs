using System;
using UniFlow.Message;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    public class CreateInstance : ConnectorBase, IValuePublisher<GameObject>
    {
        [SerializeField] private GameObject source = default;
        private GameObject Source => source == default ? gameObject : source;

        [SerializeField] private Transform parent = default;
        private Transform Parent => parent == default ? transform : parent;

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return Observable
                .Return(
                    EventMessage.Create(
                        ConnectorType.Custom,
                        this,
                        CreateInstanceMessage.Create(this)
                    )
                );
        }

        GameObject IValuePublisher<GameObject>.Publish()
        {
            return Instantiate(Source, Parent);
        }
    }
}