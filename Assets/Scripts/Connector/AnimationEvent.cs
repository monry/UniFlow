using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace EventConnector.Connector
{
    // AnimationEvent cannot fire to Component attaching to another GameObject
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu("Event Connector/AnimationEvent")]
    public class AnimationEvent : EventConnector, IEventPublisher
    {
        private ISubject<UnityEngine.AnimationEvent> Subject { get; } = new Subject<UnityEngine.AnimationEvent>();

        IObservable<EventMessage> IEventPublisher.OnPublishAsObservable() =>
            Subject
                .Take(1)
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