using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueCombiner
{
    [AddComponentMenu("UniFlow/ValueCombiner/Vector3Int", (int) ConnectorType.ValueCombinerVector3Int)]
    public class Vector3IntCombiner : ConnectorBase
    {
        [SerializeField] private PublishVector3IntEvent publisher = new PublishVector3IntEvent();

        [ValueReceiver] private int X { get; set; }
        [ValueReceiver] private int Y { get; set; }
        [ValueReceiver] private int Z { get; set; }
        [ValuePublisher("Value")] private UnityEvent<Vector3Int> Publisher => publisher;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Publisher.Invoke(new Vector3Int(X, Y, Z));
            return Observable.ReturnUnit();
        }
    }
}
