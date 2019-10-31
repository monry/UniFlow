using System.Collections.Generic;
using UniFlow.Signal;
using UnityEngine;

namespace UniFlow.Connector.SignalPublisher
{
    [AddComponentMenu("UniFlow/SignalPublisher/MusicPitchSignalPublisher", (int) ConnectorType.MusicPitchSignalPublisher)]
    public class MusicPitchSignalPublisher : SignalPublisherBase<MusicPitchSignal>, IMessageCollectable
    {
        [SerializeField] private float pitch = default;
        [SerializeField] private float duration = default;

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

        [SerializeField] private FloatCollector pitchCollector = new FloatCollector();
        [SerializeField] private FloatCollector durationCollector = new FloatCollector();

        private FloatCollector PitchCollector => pitchCollector;
        private FloatCollector DurationCollector => durationCollector;

        protected override MusicPitchSignal GetSignal() =>
            MusicPitchSignal.Create(Pitch, Duration);

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(PitchCollector, x => Pitch = x, nameof(Pitch)),
                CollectableMessageAnnotationFactory.Create(DurationCollector, x => Duration = x, nameof(Duration)),
            };
    }
}
