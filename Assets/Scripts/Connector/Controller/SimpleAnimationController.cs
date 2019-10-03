using System;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/SimpleAnimationController", (int) ConnectorType.SimpleAnimationController)]
    public class SimpleAnimationController : ConnectorBase
    {
        [SerializeField] private GameObject baseGameObject = default;
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
        [ValueReceiver] public Animator Animator
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

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<Unit> OnConnectAsObservable()
        {
            InvokeSimpleAnimationMethod();
            return Observable.ReturnUnit();
        }

        private void Awake()
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
    }

    public enum SimpleAnimationControlMethod
    {
        Play,
        Stop,
    }
}
