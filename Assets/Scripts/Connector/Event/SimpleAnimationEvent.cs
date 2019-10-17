using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/SimpleAnimationEvent", (int) ConnectorType.SimpleAnimationEvent)]
    public class SimpleAnimationEvent : ConnectorBase, IBaseGameObjectSpecifyable, IMessageCollectable
    {
        private const string MessageParameterKey = "SimpleAnimationEvent";

        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private Animator animator = default;
        [SerializeField] private SimpleAnimation simpleAnimation = default;
        [SerializeField] private SimpleAnimationEventType simpleAnimationEventType = SimpleAnimationEventType.Play;
        [SerializeField]
        [Tooltip("If you do not specify it will not be filtered")]
        private AnimationClip animationClip = default;

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
        private SimpleAnimationEventType SimpleAnimationEventType => simpleAnimationEventType;
        private AnimationClip AnimationClip
        {
            get => animationClip;
            set => animationClip = value;
        }

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

        private ISubject<(SimpleAnimationEventType eventType, AnimationClip animationClip)> CurrentStateSubject { get; } = new Subject<(SimpleAnimationEventType, AnimationClip)>();

        private IDictionary<SimpleAnimation.State, bool> PlayingStatuses { get; } = new Dictionary<SimpleAnimation.State, bool>();

        public override IObservable<Message> OnConnectAsObservable()
        {
            ObserveSimpleAnimation();
            return CurrentStateSubject
                .Where(x => (AnimationClip == default || x.animationClip == AnimationClip) && x.eventType == SimpleAnimationEventType)
                .Select(x => this.CreateMessage(x, MessageParameterKey));
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
            if (pair.Current.isPlaying && !PlayingStatuses[state])
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

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                CollectableMessageAnnotation<GameObject>.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotation<string>.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotation<Animator>.Create(AnimatorCollector, x => Animator = x),
                CollectableMessageAnnotation<SimpleAnimation>.Create(SimpleAnimationCollector, x => SimpleAnimation = x),
                CollectableMessageAnnotation<AnimationClip>.Create(AnimationClipCollector, x => AnimationClip = x),
            };
    }

    public enum SimpleAnimationEventType
    {
        Play,
        Stop,
    }
}
