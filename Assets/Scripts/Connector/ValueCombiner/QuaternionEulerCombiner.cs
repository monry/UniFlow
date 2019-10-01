using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueCombiner
{
    [AddComponentMenu("UniFlow/ValueCombiner/QuaternionEuler", (int) ConnectorType.ValueCombinerQuaternionEuler)]
    public class QuaternionEulerCombiner : ConnectorBase
    {
        [SerializeField] private PublishQuaternionEvent publisher = new PublishQuaternionEvent();

        [ValueReceiver] private Vector3 EulerAngle { get; set; }
        [ValuePublisher("Value")] private UnityEvent<Quaternion> Publisher => publisher;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Publisher.Invoke(Quaternion.Euler(EulerAngle));
            return Observable.ReturnUnit();
        }
    }
}
