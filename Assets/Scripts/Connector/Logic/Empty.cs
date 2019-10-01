using System;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Logic
{
    [AddComponentMenu("UniFlow/Logic/Empty", (int) ConnectorType.Empty)]
    public class Empty : ConnectorBase
    {
        public override IObservable<Unit> OnConnectAsObservable()
        {
            return Observable.ReturnUnit();
        }
    }
}
