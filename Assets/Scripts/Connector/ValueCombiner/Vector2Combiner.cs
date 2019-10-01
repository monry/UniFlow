using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueCombiner
{
    [AddComponentMenu("UniFlow/ValueCombiner/Vector2", (int) ConnectorType.ValueCombinerVector2)]
    public class Vector2Combiner : ConnectorBase
    {
        [SerializeField] private PublishVector2Event publisher = new PublishVector2Event();

        [ValueReceiver] private float X { get; set; }
        [ValueReceiver] private float Y { get; set; }
        [ValuePublisher("Value")] private UnityEvent<Vector2> Publisher => publisher;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Publisher.Invoke(new Vector2(X, Y));
            return Observable.ReturnUnit();
        }
    }
}
