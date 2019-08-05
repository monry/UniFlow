using UnityEngine;

namespace EventConnector.Connector
{
    // AnimationEvent cannot fire to Component attaching to another GameObject
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu("Event Connector/AnimationEvent")]
    public class AnimationEvent : EventConnector
    {
        private EventMessages EventMessages { get; set; } = EventMessages.Create();

        protected override void Connect(EventMessages eventMessages) =>
            EventMessages = eventMessages;

        /// <summary>
        /// Invoked from AnimationEvent
        /// </summary>
        /// <param name="animationEvent"></param>
        public void Dispatch(UnityEngine.AnimationEvent animationEvent)
        {
            OnConnect(EventMessages.Append(EventMessage.Create(EventType.AnimationEvent, this, animationEvent)));
        }
    }
}