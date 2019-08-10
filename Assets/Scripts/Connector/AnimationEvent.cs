using System;
using System.Linq;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/AnimationEvent", 302)]
    public class AnimationEvent : ConnectorBase
    {
        [SerializeField]
        [Tooltip("If you do not specify it will be used SimpleAnimation setting")]
        private AnimationClip animationClip = default;
        private AnimationClip AnimationClip => animationClip;

        [SerializeField] private AnimatorCullingMode cullingMode = AnimatorCullingMode.AlwaysAnimate;
        private AnimatorCullingMode CullingMode => cullingMode;

        [SerializeField] private AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal;
        private AnimatorUpdateMode UpdateMode => updateMode;

        private Animator animator = default;
        private Animator Animator
        {
            get =>
                animator != default
                    ? animator
                    : animator =
                        GetComponent<Animator>() != default
                            ? GetComponent<Animator>()
                            : gameObject.AddComponent<Animator>();
            [UsedImplicitly]
            set => animator = value;
        }

        private SimpleAnimation simpleAnimation = default;
        private SimpleAnimation SimpleAnimation
        {
            get =>
                simpleAnimation != default
                    ? simpleAnimation
                    : simpleAnimation =
                        GetComponent<SimpleAnimation>() != default
                            ? GetComponent<SimpleAnimation>()
                            : gameObject.AddComponent<SimpleAnimation>()
            ;
            [UsedImplicitly]
            set => simpleAnimation = value;
        }

        private ISubject<UnityEngine.AnimationEvent> Subject { get; } = new Subject<UnityEngine.AnimationEvent>();

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            if (AnimationClip != default && GetComponent<Animator>() == default && SimpleAnimation.GetStates().All(x => x.clip != AnimationClip))
            {
                SimpleAnimation.AddClip(AnimationClip, AnimationClip.GetInstanceID().ToString());
            }

            return Subject
                // Prevents the previous flow from being re-invoked when triggered multiple times
                .Take(1)
                .Select(x => EventMessage.Create(ConnectorType.AnimationEvent, this, x));
        }

        /// <summary>
        /// Invoked from AnimationEvent
        /// </summary>
        /// <param name="animationEvent"></param>
        [UsedImplicitly]
        public void Dispatch(UnityEngine.AnimationEvent animationEvent)
        {
            Subject.OnNext(animationEvent);
        }

        private void Awake()
        {
            // ReSharper disable once InvertIf
            // Automatic add components Animator and SimpleAnimation if AudioClip specified and Animator component does not exists.
            if (AnimationClip != default && Animator == default && SimpleAnimation.GetStates().All(x => x.clip != AnimationClip))
            {
                SimpleAnimation.AddClip(AnimationClip, AnimationClip.GetInstanceID().ToString());
                SimpleAnimation.cullingMode = CullingMode;
                Animator.updateMode = UpdateMode;
            }
        }
    }
}