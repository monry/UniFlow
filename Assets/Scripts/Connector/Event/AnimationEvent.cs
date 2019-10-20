using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/AnimationEvent", (int) ConnectorType.AnimationEvent)]
    public class AnimationEvent : ConnectorBase, IBaseGameObjectSpecifyable, IMessageCollectable, IMessageComposable
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

        public GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            private set => baseGameObject = value;
        }
        public string TransformPath
        {
            get => transformPath;
            private set => transformPath = value;
        }
        private Animator Animator
        {
            get => animator != default ? animator : animator = this.GetOrAddComponent<Animator>();
            set => animator = value;
        }
        private SimpleAnimation SimpleAnimation
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
        private AnimationClip AnimationClip
        {
            get => animationClip;
            set => animationClip = value;
        }
        private AnimatorCullingMode CullingMode => cullingMode;
        private AnimatorUpdateMode UpdateMode => updateMode;

        [SerializeField] private GameObjectCollector baseGameObjectCollector = new GameObjectCollector();
        [SerializeField] private StringCollector transformPathCollector = new StringCollector();
        [SerializeField] private AnimatorCollector animatorCollector = new AnimatorCollector();
        [SerializeField] private SimpleAnimationCollector simpleAnimationCollector = new SimpleAnimationCollector();
        [SerializeField] private AnimationClipCollector animationClipCollector = new AnimationClipCollector();
        // TODO: Implement EnumCollector

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector;
        private StringCollector TransformPathCollector => transformPathCollector;
        private AnimatorCollector AnimatorCollector => animatorCollector;
        private SimpleAnimationCollector SimpleAnimationCollector => simpleAnimationCollector;
        private AnimationClipCollector AnimationClipCollector => animationClipCollector;

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

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotationFactory.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotationFactory.Create(AnimatorCollector, x => Animator = x),
                CollectableMessageAnnotationFactory.Create(SimpleAnimationCollector, x => SimpleAnimation = x),
                CollectableMessageAnnotationFactory.Create(AnimationClipCollector, x => AnimationClip = x),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create(() => LatestAnimationEvent),
            };
    }
}
