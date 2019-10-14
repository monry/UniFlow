using System;
using System.Collections.Generic;
using System.Linq;
using UniFlow.Utility;
using UniFlow.Attribute;
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

        public override IObservable<Message> OnConnectAsObservable()
        {
            return Value ? ObservableFactory.ReturnMessage(this) : ObservableFactory.EmptyMessage();
        }
    }
}
