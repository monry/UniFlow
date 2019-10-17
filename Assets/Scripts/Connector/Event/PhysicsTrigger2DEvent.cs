using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/PhysicsTrigger2DEvent", (int) ConnectorType.PhysicsTrigger2DEvent)]
    public class PhysicsTrigger2DEvent : ConnectorBase, IMessageCollectable
    {
        [SerializeField] private Component component = default;
        [SerializeField] private PhysicsTrigger2DEventType physicsTrigger2DEventType = PhysicsTrigger2DEventType.TriggerEnter2D;

        private Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }
        private PhysicsTrigger2DEventType PhysicsTrigger2DEventType => physicsTrigger2DEventType;

        [SerializeField] private ComponentCollector componentCollector = default;

        private ComponentCollector ComponentCollector => componentCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return OnEventAsObservable()
                .Select(this.CreateMessage);
        }

        private IObservable<Collider2D> OnEventAsObservable()
        {
            switch (PhysicsTrigger2DEventType)
            {
                case PhysicsTrigger2DEventType.TriggerEnter2D:
                    return Component.OnTriggerEnter2DAsObservable();
                case PhysicsTrigger2DEventType.TriggerExit2D:
                    return Component.OnTriggerExit2DAsObservable();
                case PhysicsTrigger2DEventType.TriggerStay2D:
                    return Component.OnTriggerStay2DAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotation<Component>.Create(ComponentCollector, x => Component = x),
            };
    }

    public enum PhysicsTrigger2DEventType
    {
        TriggerEnter2D,
        TriggerExit2D,
        TriggerStay2D,
    }
}
