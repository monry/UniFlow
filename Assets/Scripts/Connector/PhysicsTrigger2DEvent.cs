using System;
using EventConnector.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace EventConnector.Connector
{
    public class PhysicsTrigger2DEvent : EventConnector
    {
        [SerializeField] private PhysicsTrigger2DEventType physicsTrigger2DEventType;
        private PhysicsTrigger2DEventType PhysicsTrigger2DEventType => physicsTrigger2DEventType;

        [SerializeField]
        [Tooltip("If you do not specify it will be used self instance")]
        private Component component;
        private Component Component => component ? component : component = this;

        protected override IObservable<EventMessages> Connect(EventMessages eventMessages)
        {
            return OnEventAsObservable().Select(x => eventMessages.Append((Component, PhysicsTrigger2DEventData.Create(PhysicsTrigger2DEventType, x))));
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