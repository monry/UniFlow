using System;
using System.Collections.Generic;
using UniFlow;
using UniRx;
using UnityEngine;

namespace PretendLand.Edwin.Scripts.Connector.Complex
{
    [AddComponentMenu("UniFlow/Complex/MusicDuckingController", (int) ConnectorType.MusicDuckingController)]
    public class MusicDuckingController : ConnectorBase, IMessageCollectable
    {
        [SerializeField] private AudioSource audioSource = default;
        [SerializeField] private float volume = default;
        [SerializeField] private float duration = default;

        private AudioSource AudioSource
        {
            get => audioSource != default ? audioSource : audioSource = MusicPlayer.BaseGameObject.GetOrAddComponent<AudioSource>();
            set => audioSource = value;
        }
        private float Volume
        {
            get => volume;
            set => volume = value;
        }
        private float Duration
        {
            get => duration;
            set => duration = value;
        }

        [SerializeField] private AudioSourceCollector audioSourceCollector = new AudioSourceCollector();
        [SerializeField] private FloatCollector volumeCollector = new FloatCollector();
        [SerializeField] private FloatCollector durationCollector = new FloatCollector();

        private AudioSourceCollector AudioSourceCollector => audioSourceCollector;
        private FloatCollector VolumeCollector => volumeCollector;
        private FloatCollector DurationCollector => durationCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            var subject = new Subject<Unit>();
            ObservableTween
                .Tween(() => AudioSource.volume, () => Volume, Duration, ObservableTween.EaseType.InOutSinusoidal)
                .Subscribe(
                    x => AudioSource.volume = x,
                    () => subject.OnNext(Unit.Default)
                );
            return subject.AsMessageObservable(this);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(AudioSourceCollector, x => AudioSource = x),
                CollectableMessageAnnotationFactory.Create(VolumeCollector, x => Volume = x, nameof(Volume)),
                CollectableMessageAnnotationFactory.Create(DurationCollector, x => Duration = x, nameof(Duration)),
            };
    }
}
