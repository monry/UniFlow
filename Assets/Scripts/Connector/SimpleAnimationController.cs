using System;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("Event Connector/SimpleAnimationController", 300)]
    public class SimpleAnimationController : EventPublisher
    {
        [SerializeField] private SimpleAnimationControlMethod simpleAnimationControlMethod = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by GameObject.GetComponent<SimpleAnimation>()")]
        private SimpleAnimation simpleAnimation = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be used SimpleAnimation setting")]
        private AnimationClip animationClip = default;

        private SimpleAnimationControlMethod SimpleAnimationControlMethod => simpleAnimationControlMethod;
        private AnimationClip AnimationClip => animationClip;
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

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<EventMessage> OnPublishAsObservable() =>
            Observable
                .Create<EventMessage>(
                    observer =>
                    {
                        InvokeSimpleAnimationMethod();
                        observer.OnNext(EventMessage.Create(EventType.SimpleAnimationController, SimpleAnimation, SimpleAnimationControllerEventData.Create(SimpleAnimationControlMethod)));
                        return Disposable;
                    }
                );

        protected override void Start()
        {
            base.Start();

            if (AnimationClip != default && SimpleAnimation.GetStates().All(x => x.clip != AnimationClip))
            {
                SimpleAnimation.AddClip(AnimationClip, AnimationClip.GetInstanceID().ToString());
            }
        }

        private void InvokeSimpleAnimationMethod()
        {
            switch (SimpleAnimationControlMethod)
            {
                case SimpleAnimationControlMethod.Play:
                    if (AnimationClip == default)
                    {
                        SimpleAnimation.Play();
                    }
                    else
                    {
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