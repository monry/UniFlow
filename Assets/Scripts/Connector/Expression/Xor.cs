using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Expression
{
    [AddComponentMenu("UniFlow/Expression/Xor", (int) ConnectorType.Xor)]
    public class Xor : ConnectorBase
    {
        [ValueReceiver] public bool Left { get; set; }
        [ValueReceiver] public bool Right { get; set; }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return Left ^ Right ? Observable.ReturnUnit() : Observable.Empty<Unit>();
        }
    }
}
