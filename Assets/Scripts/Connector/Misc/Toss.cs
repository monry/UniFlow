using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Misc
{
    [AddComponentMenu("UniFlow/Misc/Toss", (int) ConnectorType.Toss)]
    public class Toss : ConnectorBase
    {
        [SerializeField] private List<GameObject> targets = default;
        private IEnumerable<GameObject> Targets => targets;

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            foreach (var target in Targets)
            {
                if (target != default && target.activeSelf)
                {
                    (target.GetComponent<Receive>() as IConnector)?.Connect(Observable.Return(((IMessage) default, Messages.Create())));
                }
            }
            return Observable.Return(Message.Create(this));
        }

        public class Message : MessageBase<Toss>
        {
            public static Message Create(Toss sender)
            {
                return Create<Message>(ConnectorType.Toss, sender);
            }
        }
    }
}