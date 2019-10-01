using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueExtractor
{
    [AddComponentMenu("UniFlow/ValueExtractor/QuaternionEuler", (int) ConnectorType.ValueExtractorQuaternionEuler)]
    public class QuaternionEulerExtractor : ConnectorBase
    {
        [SerializeField] private PublishVector3Event publisher = new PublishVector3Event();

        [ValueReceiver] private Quaternion Value { get; set; }
        [ValuePublisher("Value")] private UnityEvent<Vector3> Publisher => publisher;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Publisher.Invoke(Value.eulerAngles);
            return Observable.ReturnUnit();
        }
    }
}
