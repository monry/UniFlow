using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/PhysicsTrigger2DEvent", (int) ConnectorType.PhysicsTrigger2DEvent)]
    public class PhysicsTrigger2DEvent : ConnectorBase
    {
        [SerializeField] private PhysicsTrigger2DEventType physicsTrigger2DEventType = (PhysicsTrigger2DEventType) (-1);
        [SerializeField] private Component component = default;

        [UsedImplicitly] public PhysicsTrigger2DEventType PhysicsTrigger2DEventType
        {
            get => physicsTrigger2DEventType;
            set => physicsTrigger2DEventType = value;
        }
        [UsedImplicitly] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return OnEventAsObservable()
                .Select(x => EventMessage.Create(ConnectorType.PhysicsTrigger2DEvent, Component, PhysicsTrigger2DEventData.Create(PhysicsTrigger2DEventType, x)));
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