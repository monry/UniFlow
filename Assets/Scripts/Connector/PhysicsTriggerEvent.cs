using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/PhyticsTriggerEvent", (int) ConnectorType.PhysicsTriggerEvent)]
    public class PhysicsTriggerEvent : ConnectorBase
    {
        [SerializeField] private PhysicsTriggerEventType physicsTriggerEventType = default;
        [SerializeField] private Component component = default;

        [UsedImplicitly] public PhysicsTriggerEventType PhysicsTriggerEventType
        {
            get => physicsTriggerEventType;
            set => physicsTriggerEventType = value;
        }
        [UsedImplicitly] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return OnEventAsObservable()
                .Select(x => EventMessage.Create(ConnectorType.PhysicsTriggerEvent, Component, PhysicsTriggerEventData.Create(PhysicsTriggerEventType, x)));
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
    }

    public enum PhysicsTriggerEventType
    {
        TriggerEnter,
        TriggerExit,
        TriggerStay,
    }
}