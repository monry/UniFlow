using System;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Signal;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/TimelineEvent", (int) ConnectorType.TimelineEvent)]
    public class TimelineEvent : ConnectorBase
    {
        [SerializeField] private TimelineEventType timelineEventType = TimelineEventType.Play;
        [SerializeField] private TimelineAsset timelineAsset = default;
        [SerializeField] private PlayableDirector playableDirector = default;

        [UsedImplicitly] public TimelineEventType TimelineEventType
        {
            get => timelineEventType;
            set => timelineEventType = value;
        }
        [UsedImplicitly] public TimelineAsset TimelineAsset
        {
            get => timelineAsset;
            set => timelineAsset = value;
        }
        [UsedImplicitly] public PlayableDirector PlayableDirector
        {
            get =>
                playableDirector != default
                    ? playableDirector
                    : playableDirector =
                        GetComponent<PlayableDirector>() != default
                            ? GetComponent<PlayableDirector>()
                            : gameObject.AddComponent<PlayableDirector>();
            set => playableDirector = value;
        }

        private static Begin beginSignal = default;
        private static End endSignal = default;
        private static Begin BeginSignal
        {
            get
            {
                // ReSharper disable once InvertIf
                if (beginSignal == default)
                {
                    beginSignal = ScriptableObject.CreateInstance<Begin>();
                    beginSignal.IsTemporaryInstance = true;
                }
                return beginSignal;
            }
        }

        private static End EndSignal
        {
            get
            {
                // ReSharper disable once InvertIf
                if (endSignal == default)
                {
                    endSignal = ScriptableObject.CreateInstance<End>();
                    endSignal.IsTemporaryInstance = true;
                }
                return endSignal;
            }
        }

        private ISubject<TimelineEventType> SignalEmittedSubject { get; } = new Subject<TimelineEventType>();

        protected override void Start()
        {
            base.Start();
            RegisterSignal();
        }

        private void OnDestroy()
        {
            if (!(PlayableDirector.playableAsset is TimelineAsset timeline))
            {
                return;
            }

            foreach (var marker in timeline.markerTrack.GetMarkers().OfType<SignalEmitter>().Where(x => x.asset is SignalAssetBase signalAsset && signalAsset.IsTemporaryInstance))
            {
                timeline.markerTrack.DeleteMarker(marker);
            }
        }

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            return SignalEmittedSubject.Select(_ => Message.Create(this));
        }

        private void RegisterSignal()
        {
            if (!(PlayableDirector.playableAsset is TimelineAsset timeline))
            {
                return;
            }

            var time = TimelineEventType == TimelineEventType.Play ? 0 : timeline.duration;
            var signal = TimelineEventType == TimelineEventType.Play ? (SignalAsset) BeginSignal : EndSignal;
            var emitter = timeline
                .markerTrack
                .GetMarkers()
                .FirstOrDefault(
                    x => Math.Abs(x.time - time) < double.Epsilon
                        && x is SignalEmitter signalEmitter
                        && signalEmitter.asset.GetType() == signal.GetType()
                ) as SignalEmitter;
            if (emitter == default)
            {
                emitter = timeline.markerTrack.CreateMarker<SignalEmitter>(time);
                emitter.asset = signal;
            }

            var receiver = PlayableDirector.gameObject.GetComponent<SignalReceiver>();
            if (receiver == default)
            {
                receiver = PlayableDirector.gameObject.AddComponent<SignalReceiver>();
            }

            if (receiver.GetReaction(signal) == default)
            {
                receiver.AddReaction(signal, new UnityEvent());
            }

            receiver.GetReaction(signal).AddListener(TimelineEventType == TimelineEventType.Play ? (UnityAction) DispatchBegin : DispatchEnd);
        }

        private void DispatchBegin()
        {
            SignalEmittedSubject.OnNext(TimelineEventType.Play);
        }

        private void DispatchEnd()
        {
            SignalEmittedSubject.OnNext(TimelineEventType.Stop);
        }

        public class Message : MessageBase<TimelineEvent>
        {
            public static Message Create(TimelineEvent sender)
            {
                return Create<Message>(ConnectorType.TimelineEvent, sender);
            }
        }
    }

    public enum TimelineEventType
    {
        Play,
        Stop,
    }
}