using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Utility;
using UniFlow.Attribute;
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
        [UsedImplicitly] public SimpleAnimationControlMethod SimpleAnimationControlMethod
        {
            get => simpleAnimationControlMethod;
            set => simpleAnimationControlMethod = value;
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

        [SerializeField] private GameObjectCollector baseGameObjectCollector = default;
        [SerializeField] private StringCollector transformPathCollector = default;
        [SerializeField] private AnimatorCollector animatorCollector = default;
        [SerializeField] private SimpleAnimationCollector simpleAnimationCollector = default;
        [SerializeField] private AnimationClipCollector animationClipCollector = default;
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
            new ICollectableMessageAnnotation[]
            {
                new CollectableMessageAnnotation<GameObject>(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                new CollectableMessageAnnotation<string>(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                new CollectableMessageAnnotation<Animator>(AnimatorCollector, x => Animator = x),
                new CollectableMessageAnnotation<SimpleAnimation>(SimpleAnimationCollector, x => SimpleAnimation = x),
                new CollectableMessageAnnotation<AnimationClip>(AnimationClipCollector, x => AnimationClip = x),
            };
    }

    public enum SimpleAnimationControlMethod
    {
        Play,
        Stop,
    }
}
