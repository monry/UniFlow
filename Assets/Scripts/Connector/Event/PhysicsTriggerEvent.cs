using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/PhysicsTriggerEvent", (int) ConnectorType.PhysicsTriggerEvent)]
    public class PhysicsTriggerEvent : ConnectorBase, IMessageCollectable
    {
        [SerializeField] private Component component = default;
        [SerializeField] private PhysicsTriggerEventType physicsTriggerEventType = PhysicsTriggerEventType.TriggerEnter;

        private Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }
        private PhysicsTriggerEventType PhysicsTriggerEventType => physicsTriggerEventType;

        [SerializeField] private ComponentCollector componentCollector = new ComponentCollector();

        private ComponentCollector ComponentCollector => componentCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return OnEventAsObservable()
                .Select(this.CreateMessage);
        }

        private IObservable<Collider> OnEventAsObservable()
        {
            switch (PhysicsTriggerEventType)
            {
                case PhysicsTriggerEventType.TriggerEnter:
                    return Component.OnTriggerEnterAsObservable();
                case PhysicsTriggerEventType.TriggerExit:
                    return Component.OnTriggerExitAsObservable();
                case PhysicsTriggerEventType.TriggerStay:
                    return Component.OnTriggerStayAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(ComponentCollector, x => Component = x),
            };
    }

    public enum PhysicsTriggerEventType
    {
        TriggerEnter,
        TriggerExit,
        TriggerStay,
    }
}
