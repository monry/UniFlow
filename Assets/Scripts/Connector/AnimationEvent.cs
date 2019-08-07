using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace EventConnector.Connector
{
    // AnimationEvent cannot fire to Component attaching to another GameObject
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu("Event Connector/AnimationEvent")]
    public class AnimationEvent : EventConnector
    {
        private ISubject<UnityEngine.AnimationEvent> Subject { get; } = new Subject<UnityEngine.AnimationEvent>();
        private EventMessages EventMessages { get; set; } = EventMessages.Create();

        public override IObservable<EventMessage> FooAsObservable() =>
            Subject
                .Select(x => EventMessage.Create(EventType.AnimationEvent, this, x));

        /// <summary>
        /// Invoked from AnimationEvent
        /// </summary>
        /// <param name="animationEvent"></param>
        [UsedImplicitly]
        public void Dispatch(UnityEngine.AnimationEvent animationEvent)
        {
            Subject.OnNext(animationEvent);
        }
    }
}