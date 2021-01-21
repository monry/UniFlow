using System;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/DontDestroyOnLoad", (int) ConnectorType.DontDestroyOnLoad)]
    public class DontDestroyOnLoad : ConnectorBase
    {
        public override IObservable<Message> OnConnectAsObservable()
        {
            DontDestroyOnLoad(gameObject);
            return ObservableFactory.ReturnMessage(this);
        }
    }
}
