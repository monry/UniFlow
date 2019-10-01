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

        public override IObservable<Unit> OnConnectAsObservable()
        {
            foreach (var target in Targets)
            {
                if (target != default && target.activeSelf)
                {
                    (target.GetComponent<Receive>() as IConnector)?.Connect(Observable.ReturnUnit());
                }
            }
            return Observable.ReturnUnit();
        }
    }
}
