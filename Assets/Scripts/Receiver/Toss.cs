using System.Collections.Generic;
using UniFlow.Connector;
using UniRx;
using UnityEngine;

namespace UniFlow.Receiver
{
    [AddComponentMenu("UniFlow/Misc/Toss", (int) ConnectorType.Toss)]
    public class Toss : ReceiverBase
    {
        [SerializeField] private List<GameObject> targets = default;
        private IEnumerable<GameObject> Targets => targets;

        public override void OnReceive(Messages messages)
        {
            foreach (var target in Targets)
            {
                if (target != default && target.activeSelf)
                {
                    (target.GetComponent<Receive>() as IConnector)?.Connect(Observable.Return(((IMessage) default, messages)));
                }
            }
        }
    }
}