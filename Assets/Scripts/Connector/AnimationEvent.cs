using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    // AnimationEvent cannot fire to Component attaching to another GameObject
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu("UniFlow/AnimationEvent", 302)]
    public class AnimationEvent : ConnectorBase
    {
        private ISubject<UnityEngine.AnimationEvent> Subject { get; } = new Subject<UnityEngine.AnimationEvent>();

        public override IObservable<EventMessage> OnPublishAsObservable() =>
            Subject
                .Take(1)
                .Select(x => EventMessage.Create(ConnectorType.AnimationEvent, this, x));

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