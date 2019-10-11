using System;
using System.Collections.Generic;
using System.Linq;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Expression
{
    [AddComponentMenu("UniFlow/Expression/Not", (int) ConnectorType.Not)]
    public class Not : ConnectorBase
    {
        private IList<bool> Conditions { get; } = new List<bool>();

        [ValueReceiver]
        public bool Value
        {
            get => Conditions.All(x => !x);
            set => Conditions.Add(value);
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return Value ? Observable.ReturnUnit() : Observable.Empty<Unit>();
        }
    }
}
