using System;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.Logic
{
    [AddComponentMenu("UniFlow/Logic/Empty", (int) ConnectorType.Empty)]
    public class Empty : ConnectorBase
    {
        public override IObservable<Message> OnConnectAsObservable()
        {
            return ObservableFactory.ReturnMessage(this);
        }
    }
}
