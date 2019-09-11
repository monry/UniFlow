using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/SimpleAnimationController", (int) ConnectorType.SimpleAnimationController)]
    public class SimpleAnimationController : ConnectorBase
    {
        [SerializeField] private SimpleAnimationControlMethod simpleAnimationControlMethod = (SimpleAnimationControlMethod) (-1);
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
                        GetComponent<Animator>() != default
                            ? GetComponent<Animator>()
                            : gameObject.AddComponent<Animator>();
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

        private IDisposable Disposable { get; } = new CompositeDisposable();

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
//            if (latestMessage is IValueHolder<SimpleAnimationControlMethod> simpleAnimationControlMethodHolder)
//            {
//                SimpleAnimationControlMethod = simpleAnimationControlMethodHolder.Value;
//            }
//
//            if (latestMessage is IValueHolder<AnimationClip> animationClipHolder)
//            {
//                AnimationClip = animationClipHolder.Value;
//            }
//
//            if (latestMessage is IValueHolder<AnimatorCullingMode> animatorCullingModeHolder)
//            {
//                CullingMode = animatorCullingModeHolder.Value;
//            }
//
//            if (latestMessage is IValueHolder<AnimatorUpdateMode> animatorUpdateModeHolder)
//            {
//                UpdateMode = animatorUpdateModeHolder.Value;
//            }

            return Observable
                .Create<IMessage>(
                    observer =>
                    {
                        InvokeSimpleAnimationMethod();
                        observer.OnNext(Message.Create(this));
                        return Disposable;
                    }
                );
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