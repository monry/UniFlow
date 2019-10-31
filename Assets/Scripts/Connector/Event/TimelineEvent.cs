using System;
using System.Collections.Generic;
using System.Linq;
using UniFlow.TimelineSignal;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/TimelineEvent", (int) ConnectorType.TimelineEvent)]
    public class TimelineEvent : ConnectorBase, IBaseGameObjectSpecifyable, IMessageCollectable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField] private PlayableDirector playableDirector = default;
        [SerializeField] private TimelineEventType timelineEventType = TimelineEventType.Play;
        [SerializeField] private TimelineAsset timelineAsset = default;

        public GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            private set
            {
                baseGameObject = value;
                transformPath = default;
                playableDirector = default;
                timelineAsset = default;
                // Try to re-register when TimelineAsset may change
                // Note: Marker is ignored if Playable has already been played, so a hack such as delaying playback by 1 frame is required
                RegisterSignal();
            }
        }

        public string TransformPath
        {
            get => transformPath;
            private set
            {
                transformPath = value;
                playableDirector = default;
                timelineAsset = default;
                // Try to re-register when TimelineAsset may change
                // Note: Marker is ignored if Playable has already been played, so a hack such as delaying playback by 1 frame is required
                RegisterSignal();
            }
        }

        private PlayableDirector PlayableDirector
        {
            get => playableDirector != default ? playableDirector : playableDirector = this.GetOrAddComponent<PlayableDirector>();
            set
            {
                playableDirector = value;
                timelineAsset = default;
                // Try to re-register when TimelineAsset may change
                // Note: Marker is ignored if Playable has already been played, so a hack such as delaying playback by 1 frame is required
                RegisterSignal();
            }
        }

        private TimelineEventType TimelineEventType => timelineEventType;
        private TimelineAsset TimelineAsset
        {
            get => timelineAsset != default ? timelineAsset : PlayableDirector.playableAsset as TimelineAsset;
            set
            {
                timelineAsset = value;
                // Try to re-register when TimelineAsset may change
                // Note: Marker is ignored if Playable has already been played, so a hack such as delaying playback by 1 frame is required
                RegisterSignal();
            }
        }

        [SerializeField] private GameObjectCollector baseGameObjectCollector = new GameObjectCollector();
        [SerializeField] private StringCollector transformPathCollector = new StringCollector();
        [SerializeField] private PlayableDirectorCollector playableDirectorCollector = new PlayableDirectorCollector();
        [SerializeField] private TimelineAssetCollector timelineAssetCollector = new TimelineAssetCollector();

        private GameObjectCollector BaseGameObjectCollector => baseGameObjectCollector;
        private StringCollector TransformPathCollector => transformPathCollector;
        private PlayableDirectorCollector PlayableDirectorCollector => playableDirectorCollector;
        private TimelineAssetCollector TimelineAssetCollector => timelineAssetCollector;

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

        private MarkerTrack MarkerTrack { get; set; }

        private void Awake()
        {
            RegisterSignal();
        }

        private void OnDestroy()
        {
            if (MarkerTrack == default)
            {
                return;
            }

            foreach (var marker in MarkerTrack.GetMarkers().OfType<SignalEmitter>().Where(x => x.asset is SignalAssetBase signalAsset && signalAsset.IsTemporaryInstance))
            {
                MarkerTrack.DeleteMarker(marker);
            }
        }

        public override IObservable<Message> OnConnectAsObservable()
        {
            return SignalEmittedSubject
                .Select(this.CreateMessage);
        }

        private void RegisterSignal()
        {
            if (!(TimelineAsset is TimelineAsset timeline))
            {
                return;
            }

            var time = TimelineEventType == TimelineEventType.Play ? 0 : timeline.duration;
            var signal = TimelineEventType == TimelineEventType.Play ? (SignalAsset) BeginSignal : EndSignal;
            MarkerTrack = timeline.markerTrack;
            var emitter = MarkerTrack
                .GetMarkers()
                .FirstOrDefault(
                    x => x is SignalEmitter signalEmitter
                        && signalEmitter != default
                        && Math.Abs(signalEmitter.time - time) < double.Epsilon
                        && signalEmitter.asset != default
                        && signalEmitter.asset.GetType() == signal.GetType()
                ) as SignalEmitter;
            if (emitter == default)
            {
                emitter = MarkerTrack.CreateMarker<SignalEmitter>(time);
                emitter.asset = signal;
            }

            var receiver = PlayableDirector.gameObject.GetComponent<UnityEngine.Timeline.SignalReceiver>();
            if (receiver == default)
            {
                receiver = PlayableDirector.gameObject.AddComponent<UnityEngine.Timeline.SignalReceiver>();
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

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(BaseGameObjectCollector, x => BaseGameObject = x, nameof(BaseGameObject)),
                CollectableMessageAnnotationFactory.Create(TransformPathCollector, x => TransformPath = x, nameof(TransformPath)),
                CollectableMessageAnnotationFactory.Create(PlayableDirectorCollector, x => PlayableDirector = x),
                CollectableMessageAnnotationFactory.Create(TimelineAssetCollector, x => TimelineAsset = x),
            };
    }

    public enum TimelineEventType
    {
        Play,
        Stop,
    }
}
