using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/AnimationEvent", (int) ConnectorType.AnimationEvent)]
    public class AnimationEvent : ConnectorBase, IBaseGameObjectSpecifyable, IMessageComposable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private Animator animator = default;
        [SerializeField] private SimpleAnimation simpleAnimation = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be used SimpleAnimation setting")]
        private AnimationClip animationClip = default;
        [SerializeField] private AnimatorCullingMode cullingMode = AnimatorCullingMode.AlwaysAnimate;
        [SerializeField] private AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal;

        [ValueReceiver] public GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }
        [ValueReceiver] public string TransformPath
        {
            get => transformPath;
            set => transformPath = value;
        }
        [ValueReceiver] public Animator Animator
        {
            get => animator != default ? animator : animator = this.GetOrAddComponent<Animator>();
            set => animator = value;
        }
        [ValueReceiver] public SimpleAnimation SimpleAnimation
        {
            get =>
                simpleAnimation != default
                    ? simpleAnimation
                    : simpleAnimation =
                        Animator.GetComponent<SimpleAnimation>() != default
                            ? Animator.GetComponent<SimpleAnimation>()
                            : Animator.gameObject.AddComponent<SimpleAnimation>()
            ;
            set => simpleAnimation = value;
        }
        [ValueReceiver] public AnimationClip AnimationClip
        {
            get => animationClip;
            set => animationClip = value;
        }
        [ValueReceiver] public AnimatorCullingMode CullingMode
        {
            get => cullingMode;
            set => cullingMode = value;
        }
        [ValueReceiver] public AnimatorUpdateMode UpdateMode
        {
            get => updateMode;
            set => updateMode = value;
        }

        private ISubject<UnityEngine.AnimationEvent> Subject { get; } = new Subject<UnityEngine.AnimationEvent>();

        private UnityEngine.AnimationEvent LatestAnimationEvent { get; set; }

        public override IObservable<Message> OnConnectAsObservable()
        {
            PrepareAnimationEvent();
            return Subject
                // Prevents the previous flow from being re-invoked when triggered multiple times
                .Take(1)
                .Select(this.CreateMessage);
        }

        /// <summary>
        /// Invoked from AnimationEvent
        /// </summary>
        /// <param name="animationEvent"></param>
        [UsedImplicitly]
        public void Dispatch(UnityEngine.AnimationEvent animationEvent)
        {
            Subject.OnNext(animationEvent);
            LatestAnimationEvent = animationEvent;
        }

        private void PrepareAnimationEvent()
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

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                new ComposableMessageAnnotation<UnityEngine.AnimationEvent>(() => LatestAnimationEvent),
            };
    }
}
