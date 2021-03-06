using System;
using System.Collections.Generic;
using System.Linq;
using UniFlow.Utility;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/SimpleAnimationController", (int) ConnectorType.SimpleAnimationController)]
    public class SimpleAnimationController : ConnectorBase,
        IBaseGameObjectSpecifyable,
        IMessageCollectable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private Animator animator = default;
        [SerializeField] private SimpleAnimation simpleAnimation = default;
        [SerializeField] private SimpleAnimationControlMethod simpleAnimationControlMethod = SimpleAnimationControlMethod.Play;
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
        public SimpleAnimation SimpleAnimation
        {
            get =>
                simpleAnimation != default
                    ? simpleAnimation
                    : simpleAnimation =
                        Animator.GetComponent<SimpleAnimation>() != default
                            ? Animator.GetComponent<SimpleAnimation>()
                            : Animator.gameObject.AddComponent<SimpleAnimation>()
            ;
            private set => simpleAnimation = value;
        }
        private SimpleAnimationControlMethod SimpleAnimationControlMethod => simpleAnimationControlMethod;
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

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<Message> OnConnectAsObservable()
        {
            PrepareSimpleAnimation();
            InvokeSimpleAnimationMethod();
            return ObservableFactory.ReturnMessage(this);
        }

        private void PrepareSimpleAnimation()
        {
            // ReSharper disable once InvertIf
            // Automatic add components Animator and SimpleAnimation if AudioClip specified and Animator component does not exists.
            if (AnimationClip != default && Animator != default && SimpleAnimation.GetStates().All(x => x.clip != AnimationClip))
            {
                SimpleAnimation.AddClip(AnimationClip, AnimationClip.GetInstanceID().ToString());
                SimpleAnimation.cullingMode = CullingMode;
                SimpleAnimation.playAutomatically = false;
                Animator.updateMode = UpdateMode;
            }
        }

        private void InvokeSimpleAnimationMethod()
        {
            switch (SimpleAnimationControlMethod)
            {
                case SimpleAnimationControlMethod.Play:
                    if (AnimationClip == default)
                    {
                        SimpleAnimation.Rewind();
                        SimpleAnimation.Play();
                    }
                    else
                    {
                        SimpleAnimation.Rewind(AnimationClip.GetInstanceID().ToString());
                        SimpleAnimation.Play(AnimationClip.GetInstanceID().ToString());
                    }
                    break;
                case SimpleAnimationControlMethod.Stop:
                    if (AnimationClip == default)
                    {
                        SimpleAnimation.Stop();
                    }
                    else
                    {
                        SimpleAnimation.Stop(AnimationClip.GetInstanceID().ToString());
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
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
    }

    public enum SimpleAnimationControlMethod
    {
        Play,
        Stop,
    }
}
