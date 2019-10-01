using System;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Misc
{
    [AddComponentMenu("UniFlow/Misc/Receive", (int) ConnectorType.Receive)]
    public class Receive : ConnectorBase
    {
        // force return false to prevent triggering
        public override bool ActAsTrigger
        {
            get => false;
            set
            {
            }
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return Observable.ReturnUnit();
        }
    }
}
