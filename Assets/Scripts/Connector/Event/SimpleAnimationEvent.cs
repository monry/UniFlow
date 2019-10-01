using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniFlow.Attribute;
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
        [ValuePublisher] public AnimationClip AnimationClip
        {
            get => animationClip;
            set => animationClip = value;
        }
        [ValuePublisher] public Animator Animator
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
        [ValuePublisher] public SimpleAnimation SimpleAnimation
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

        private IDictionary<SimpleAnimation.State, bool> PlayingStatuses { get; } = new Dictionary<SimpleAnimation.State, bool>();

        private void Start()
        {
            ObserveSimpleAnimation();
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return CurrentStateSubject
                .Where(x => (AnimationClip == default || x.animationClip == AnimationClip) && x.eventType == SimpleAnimationEventType)
                .AsUnitObservable();
        }

        private void ObserveSimpleAnimation()
        {
            foreach (var state in SimpleAnimation.GetStates())
            {
                PlayingStatuses[state] = false;
                state
                    .ObserveEveryValueChanged(x => (SimpleAnimation.IsPlaying(x.name), x.normalizedTime))
                    .Pairwise()
                    .SubscribeWithState(state, OnChangeAnimatorStateInfo)
                    .AddTo(this);
            }
        }

        private void OnChangeAnimatorStateInfo(Pair<(bool isPlaying, float normalizedTime)> pair, SimpleAnimation.State state)
        {
            if (!pair.Previous.isPlaying && pair.Current.isPlaying && !PlayingStatuses[state])
            {
                PlayingStatuses[state] = true;
                CurrentStateSubject.OnNext((SimpleAnimationEventType.Play, state.clip));
            }
            else if (pair.Previous.isPlaying && !pair.Current.isPlaying && PlayingStatuses[state])
            {
                PlayingStatuses[state] = false;
                CurrentStateSubject.OnNext((SimpleAnimationEventType.Stop, state.clip));
            }
            else if (pair.Previous.normalizedTime < 1.0f && Mathf.Approximately(pair.Current.normalizedTime, 1.0f) && PlayingStatuses[state])
            {
                PlayingStatuses[state] = false;
                CurrentStateSubject.OnNext((SimpleAnimationEventType.Stop, state.clip));
            }
        }
    }

    public enum SimpleAnimationEventType
    {
        Play,
        Stop,
    }
}
