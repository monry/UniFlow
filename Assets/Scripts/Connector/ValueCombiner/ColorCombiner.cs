using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueCombiner
{
    [AddComponentMenu("UniFlow/ValueCombiner/Color", (int) ConnectorType.ValueCombinerColor)]
    public class ColorCombiner : ConnectorBase
    {
        [SerializeField] private PublishColorEvent publisher = new PublishColorEvent();

        [ValueReceiver] private float R { get; set; }
        [ValueReceiver] private float G { get; set; }
        [ValueReceiver] private float B { get; set; }
        [ValueReceiver] private float A { get; set; }
        [ValuePublisher("Value")] private UnityEvent<Color> Publisher => publisher;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Publisher.Invoke(new Color(R, G, B, A));
            return Observable.ReturnUnit();
        }
    }
}
