using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/CameraEvent", (int) ConnectorType.CameraEvent)]
    public class CameraEvent : ConnectorBase, IMessageCollectable
    {
        [SerializeField] private Component component = default;
        [SerializeField] private CameraEventType cameraEventType = CameraEventType.BecomeVisible;

        private Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }
        private CameraEventType CameraEventType => cameraEventType;

        [SerializeField] private ComponentCollector componentCollector = default;
        private ComponentCollector ComponentCollector => componentCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return OnEventAsObservable().Select(this.CreateMessage);
        }

        private IObservable<Unit> OnEventAsObservable()
        {
            switch (CameraEventType)
            {
                case CameraEventType.BecomeVisible:
                    return Component.OnBecameVisibleAsObservable();
                case CameraEventType.BecomeInvisible:
                    return Component.OnBecameInvisibleAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                new CollectableMessageAnnotation<Component>(ComponentCollector, x => Component = x),
            };
    }

    public enum CameraEventType
    {
        BecomeVisible,
        BecomeInvisible,
    }
}
