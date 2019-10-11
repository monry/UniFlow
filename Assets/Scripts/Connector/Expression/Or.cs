using System;
using System.Collections.Generic;
using System.Linq;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Expression
{
    [AddComponentMenu("UniFlow/Expression/Or", (int) ConnectorType.Or)]
    public class Or : ConnectorBase
    {
        private IList<bool> Conditions { get; } = new List<bool>();

        [ValueReceiver]
        public bool Value
        {
            get => Conditions.Any(x => x);
            set => Conditions.Add(value);
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return Value ? Observable.ReturnUnit() : Observable.Empty<Unit>();
        }
    }
}
