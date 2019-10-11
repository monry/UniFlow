using System;
using System.Collections.Generic;
using System.Linq;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Expression
{
    [AddComponentMenu("UniFlow/Expression/And", (int) ConnectorType.And)]
    public class And : ConnectorBase
    {
        private IList<bool> Conditions { get; } = new List<bool>();

        [ValueReceiver]
        public bool Value
        {
            get => Conditions.All(x => x);
            set => Conditions.Add(value);
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return Value ? Observable.ReturnUnit() : Observable.Empty<Unit>();
        }
    }
}
