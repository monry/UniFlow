using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueCombiner
{
    [AddComponentMenu("UniFlow/ValueCombiner/Color32", (int) ConnectorType.ValueCombinerColor32)]
    public class Color32Combiner : ConnectorBase
    {
        [SerializeField] private PublishColor32Event publisher = new PublishColor32Event();

        [ValueReceiver] private byte R { get; set; }
        [ValueReceiver] private byte G { get; set; }
        [ValueReceiver] private byte B { get; set; }
        [ValueReceiver] private byte A { get; set; }
        [ValuePublisher("Value")] private UnityEvent<Color32> Publisher => publisher;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Publisher.Invoke(new Color32(R, G, B, A));
            return Observable.ReturnUnit();
        }
    }
}
