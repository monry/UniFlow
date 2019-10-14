using System;
using UniFlow.Utility;
using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.Expression
{
    [AddComponentMenu("UniFlow/Expression/Xor", (int) ConnectorType.Xor)]
    public class Xor : ConnectorBase
    {
        [ValueReceiver] public bool Left { get; set; }
        [ValueReceiver] public bool Right { get; set; }

        public override IObservable<Message> OnConnectAsObservable()
        {
            return Left ^ Right ? ObservableFactory.ReturnMessage(this) : ObservableFactory.EmptyMessage();
        }
    }
}
