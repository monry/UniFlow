using System;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/PhysicsTrigger2DEvent", (int) ConnectorType.PhysicsTrigger2DEvent)]
    public class PhysicsTrigger2DEvent : ConnectorBase
    {
        [ValuePublisher] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }
        [SerializeField] private Component component = default;
        [SerializeField] private PhysicsTrigger2DEventType physicsTrigger2DEventType = PhysicsTrigger2DEventType.TriggerEnter2D;

        [UsedImplicitly] public PhysicsTrigger2DEventType PhysicsTrigger2DEventType
        {
            get => physicsTrigger2DEventType;
            set => physicsTrigger2DEventType = value;
        }

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
    }

    public enum PhysicsTrigger2DEventType
    {
        TriggerEnter2D,
        TriggerExit2D,
        TriggerStay2D,
    }
}
