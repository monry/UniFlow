using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Misc
{
    [AddComponentMenu("UniFlow/Misc/Toss", (int) ConnectorType.Toss)]
    public class Toss : ConnectorBase
    {
        [SerializeField] private List<GameObject> targets = default;
        private IList<GameObject> Targets => targets;

        [ValueReceiver] public GameObject TargetGameObject
        {
            get => null;
            set => Targets.Add(value);
        }

        public override IObservable<Message> OnConnectAsObservable()
        {
            foreach (var target in Targets)
            {
                if (target != default && target.activeSelf)
                {
                    (target.GetComponent<Receive>() as IConnector)?.Connect(ObservableFactory.ReturnMessage(this));
                }
            }
            return ObservableFactory.ReturnMessage(this);
        }
    }
}
