using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueCombiner
{
    [AddComponentMenu("UniFlow/ValueCombiner/Quaternion", (int) ConnectorType.ValueCombinerQuaternion)]
    public class QuaternionCombiner : ConnectorBase
    {
        [SerializeField] private PublishQuaternionEvent publisher = new PublishQuaternionEvent();

        [ValueReceiver] private float X { get; set; }
        [ValueReceiver] private float Y { get; set; }
        [ValueReceiver] private float Z { get; set; }
        [ValueReceiver] private float W { get; set; }
        [ValuePublisher("Value")] private UnityEvent<Quaternion> Publisher => publisher;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Publisher.Invoke(new Quaternion(X, Y, Z, W));
            return Observable.ReturnUnit();
        }
    }
}
