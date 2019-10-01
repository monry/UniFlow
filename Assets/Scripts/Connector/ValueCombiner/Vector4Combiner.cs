using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueCombiner
{
    [AddComponentMenu("UniFlow/ValueCombiner/Vector4", (int) ConnectorType.ValueCombinerVector4)]
    public class Vector4Combiner : ConnectorBase
    {
        [SerializeField] private PublishVector4Event publisher = new PublishVector4Event();

        [ValueReceiver] private float X { get; set; }
        [ValueReceiver] private float Y { get; set; }
        [ValueReceiver] private float Z { get; set; }
        [ValueReceiver] private float W { get; set; }
        [ValuePublisher("Value")] private UnityEvent<Vector4> Publisher => publisher;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Publisher.Invoke(new Vector4(X, Y, Z, W));
            return Observable.ReturnUnit();
        }
    }
}
