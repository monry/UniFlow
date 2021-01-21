using System.Collections.Generic;
using UniFlow.Signal;
using UnityEngine;

namespace UniFlow.Connector.SignalPublisher
{
    [AddComponentMenu("UniFlow/SignalPublisher/MusicDuckingSignalPublisher", (int) ConnectorType.MusicDuckingSignalPublisher)]
    public class MusicDuckingSignalPublisher : SignalPublisherBase<MusicDuckingSignal>, IMessageCollectable
    {
        [SerializeField] private float volume = default;
        [SerializeField] private float duration = default;

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

        [SerializeField] private FloatCollector volumeCollector = new FloatCollector();
        [SerializeField] private FloatCollector durationCollector = new FloatCollector();

        private FloatCollector VolumeCollector => volumeCollector;
        private FloatCollector DurationCollector => durationCollector;

        protected override MusicDuckingSignal GetSignal() =>
            MusicDuckingSignal.Create(Volume, Duration);

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(VolumeCollector, x => Volume = x, nameof(Volume)),
                CollectableMessageAnnotationFactory.Create(DurationCollector, x => Duration = x, nameof(Duration)),
            };
    }
}
