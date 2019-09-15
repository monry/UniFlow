using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/SimpleAnimationEvent", (int) ConnectorType.SimpleAnimationEvent)]
    public class SimpleAnimationEvent : ConnectorBase
    {
        [SerializeField] private SimpleAnimationEventType simpleAnimationEventType = SimpleAnimationEventType.Play;
        [SerializeField]
        [Tooltip("If you do not specify it will not be filtered")]
        private AnimationClip animationClip = default;
        [SerializeField] private Animator animator = default;
        [SerializeField] private SimpleAnimation simpleAnimation = default;

        [UsedImplicitly] public SimpleAnimationEventType SimpleAnimationEventType
        {
            get => simpleAnimationEventType;
            set => simpleAnimationEventType = value;
        }
        [UsedImplicitly] public AnimationClip AnimationClip
        {
            get => animationClip;
            set => animationClip = value;
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

        private ISubject<(SimpleAnimationEventType eventType, AnimationClip animationClip)> CurrentStateSubject { get; } = new Subject<(SimpleAnimationEventType, AnimationClip)>();

        protected override void Start()
        {
            base.Start();
            ObserveSimpleAnimation();
        }

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            return CurrentStateSubject
                .Where(x => (AnimationClip == default || x.animationClip == AnimationClip) && x.eventType == SimpleAnimationEventType)
                .Select(_ => Message.Create(this));
        }

        private void ObserveSimpleAnimation()
        {
            foreach (var state in SimpleAnimation.GetStates())
            {
                state
                    .ObserveEveryValueChanged(x => (SimpleAnimation.IsPlaying(x.name), x.normalizedTime))
                    .Pairwise()
                    .SubscribeWithState(state, OnChangeAnimatorStateInfo)
                    .AddTo(this);
            }
        }

        private void OnChangeAnimatorStateInfo(Pair<(bool isPlaying, float normalizedTime)> pair, SimpleAnimation.State state)
        {
            if (!pair.Previous.isPlaying && pair.Current.isPlaying)
            {
                CurrentStateSubject.OnNext((SimpleAnimationEventType.Play, state.clip));
            }
            else if (pair.Previous.isPlaying && !pair.Current.isPlaying)
            {
                CurrentStateSubject.OnNext((SimpleAnimationEventType.Stop, state.clip));
            }
            else if (pair.Previous.normalizedTime < 1.0f && Mathf.Approximately(pair.Current.normalizedTime, 1.0f))
            {
                CurrentStateSubject.OnNext((SimpleAnimationEventType.Stop, state.clip));
            }
        }

        public class Message : MessageBase<SimpleAnimationEvent>
        {
            public static Message Create(SimpleAnimationEvent sender)
            {
                return Create<Message>(ConnectorType.SimpleAnimationEvent, sender);
            }
        }
    }

    public enum SimpleAnimationEventType
    {
        Play,
        Stop,
    }
}