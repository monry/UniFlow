using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/SimpleAnimationController", (int) ConnectorType.SimpleAnimationController)]
    public class SimpleAnimationController : ConnectorBase
    {
        [SerializeField] private SimpleAnimationControlMethod simpleAnimationControlMethod = SimpleAnimationControlMethod.Play;
        [SerializeField]
        [Tooltip("If you do not specify it will be used SimpleAnimation setting")]
        private AnimationClip animationClip = default;
        [SerializeField] private AnimatorCullingMode cullingMode = AnimatorCullingMode.AlwaysAnimate;
        [SerializeField] private AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal;
        [SerializeField] private Animator animator = default;
        [SerializeField] private SimpleAnimation simpleAnimation = default;

        [UsedImplicitly] public SimpleAnimationControlMethod SimpleAnimationControlMethod
        {
            get => simpleAnimationControlMethod;
            set => simpleAnimationControlMethod = value;
        }
        [UsedImplicitly] public AnimationClip AnimationClip
        {
            get => animationClip;
            set => animationClip = value;
        }
        [UsedImplicitly] public AnimatorCullingMode CullingMode
        {
            get => cullingMode;
            set => cullingMode = value;
        }
        [UsedImplicitly] public AnimatorUpdateMode UpdateMode
        {
            get => updateMode;
            set => updateMode = value;
        }
        [UsedImplicitly] public Animator Animator
        {
            get =>
                animator != default
                    ? animator
                    : animator =
                        BaseGameObject.GetComponent<Animator>() != default
                            ? BaseGameObject.GetComponent<Animator>()
                            : BaseGameObject.AddComponent<Animator>();
            set => animator = value;
        }
        [UsedImplicitly] public SimpleAnimation SimpleAnimation
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

        private GameObject baseGameObject;
        [UsedImplicitly]
        [ValueReceiver]
        public GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }

        private IDisposable Disposable { get; } = new CompositeDisposable();

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            InvokeSimpleAnimationMethod();
            return Observable.Return(Message.Create(this));
        }

        [ValueReceiver]
        public GameObject Piyo { get; set; }

        [ValueReceiver]
        public Vector2 Hoge { get; set; }

        [ValueReceiver]
        public int Fuga { get; set; }

        protected override void Awake()
        {
            base.Awake();
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

        public class Message : MessageBase<SimpleAnimationController>
        {
            public static Message Create(SimpleAnimationController sender)
            {
                return Create<Message>(ConnectorType.SimpleAnimationController, sender);
            }
        }
    }

    public enum SimpleAnimationControlMethod
    {
        Play,
        Stop,
    }
}
