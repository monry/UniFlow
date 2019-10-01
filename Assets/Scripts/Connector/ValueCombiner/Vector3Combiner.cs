using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueCombiner
{
    [AddComponentMenu("UniFlow/ValueCombiner/Vector3", (int) ConnectorType.ValueCombinerVector3)]
    public class Vector3Combiner : ConnectorBase
    {
        [SerializeField] private PublishVector3Event publisher = new PublishVector3Event();

        [ValueReceiver] private float X { get; set; }
        [ValueReceiver] private float Y { get; set; }
        [ValueReceiver] private float Z { get; set; }
        [ValuePublisher("Value")] private UnityEvent<Vector3> Publisher => publisher;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Publisher.Invoke(new Vector3(X, Y, Z));
            return Observable.ReturnUnit();
        }
    }
}
