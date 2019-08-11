using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/PhyticsTriggerEvent", 202)]
    public class PhysicsTriggerEvent : ConnectorBase
    {
        [SerializeField] private PhysicsTriggerEventType physicsTriggerEventType = default;
        private PhysicsTriggerEventType PhysicsTriggerEventType
        {
            get => physicsTriggerEventType;
            [UsedImplicitly]
            set => physicsTriggerEventType = value;
        }

        private Component component = default;
        private Component Component
        {
            get => component ? component : component = this;
            [UsedImplicitly]
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