using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueCombiner
{
    [AddComponentMenu("UniFlow/ValueCombiner/Vector2Int", (int) ConnectorType.ValueCombinerVector2Int)]
    public class Vector2IntCombiner : ConnectorBase
    {
        [SerializeField] private PublishVector2IntEvent publisher = new PublishVector2IntEvent();

        [ValueReceiver] private int X { get; set; }
        [ValueReceiver] private int Y { get; set; }
        [ValuePublisher("Value")] private UnityEvent<Vector2Int> Publisher => publisher;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Publisher.Invoke(new Vector2Int(X, Y));
            return Observable.ReturnUnit();
        }
    }
}
