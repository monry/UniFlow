using System;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/DontDestroyOnLoad", (int) ConnectorType.DontDestroyOnLoad)]
    public class DontDestroyOnLoad : ConnectorBase
    {
        public override IObservable<Unit> OnConnectAsObservable()
        {
            DontDestroyOnLoad(gameObject);
            return Observable.ReturnUnit();
        }
    }
}
