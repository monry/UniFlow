using System;
using System.Collections.Generic;
using UniFlow;
using UniRx;
using UnityEngine;

namespace PretendLand.Edwin.Scripts.Connector.Complex
{
    [AddComponentMenu("UniFlow/Complex/MusicPitchController", (int) ConnectorType.MusicPitchController)]
    public class MusicPitchController : ConnectorBase, IMessageCollectable
    {
        [SerializeField] private AudioSource audioSource = default;
        [SerializeField] private float pitch = default;
        [SerializeField] private float duration = default;

        private AudioSource AudioSource
        {
            get => audioSource != default ? audioSource : audioSource = MusicPlayer.BaseGameObject.GetOrAddComponent<AudioSource>();
            set => audioSource = value;
        }
        private float Pitch
        {
            get => pitch;
            set => pitch = value;
        }
        private float Duration
        {
            get => duration;
            set => duration = value;
        }

        [SerializeField] private AudioSourceCollector audioSourceCollector = new AudioSourceCollector();
        [SerializeField] private FloatCollector pitchCollector = new FloatCollector();
        [SerializeField] private FloatCollector durationCollector = new FloatCollector();

        private AudioSourceCollector AudioSourceCollector => audioSourceCollector;
        private FloatCollector PitchCollector => pitchCollector;
        private FloatCollector DurationCollector => durationCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            var subject = new Subject<Unit>();
            ObservableTween
                .Tween(() => AudioSource.pitch, () => Pitch, Duration, ObservableTween.EaseType.InOutSinusoidal)
                .Subscribe(
                    x => AudioSource.pitch = x,
                    () => subject.OnNext(Unit.Default)
                );
            return subject.AsMessageObservable(this);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(AudioSourceCollector, x => AudioSource = x),
                CollectableMessageAnnotationFactory.Create(PitchCollector, x => Pitch = x, nameof(Pitch)),
                CollectableMessageAnnotationFactory.Create(DurationCollector, x => Duration = x, nameof(Duration)),
            };
    }
}
