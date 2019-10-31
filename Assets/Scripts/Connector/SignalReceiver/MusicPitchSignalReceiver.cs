using System.Collections.Generic;
using System.Linq;
using UniFlow.Signal;
using UnityEngine;

namespace UniFlow.Connector.SignalReceiver
{
    [AddComponentMenu("UniFlow/SignalReceiver/MusicPitchSignalReceiver", (int) ConnectorType.MusicPitchSignalReceiver)]
    public class MusicPitchSignalReceiver : SignalReceiverBase<MusicPitchSignal>
    {
        protected override MusicPitchSignal GetSignal() =>
            MusicPitchSignal.Create();

        public override IEnumerable<IComposableMessageAnnotation> GetMessageComposableAnnotations() =>
            base.GetMessageComposableAnnotations()
                .Concat(
                    new[]
                    {
                        ComposableMessageAnnotationFactory.Create(() => ReceivedSignal.Pitch, nameof(ReceivedSignal.Pitch)),
                        ComposableMessageAnnotationFactory.Create(() => ReceivedSignal.Duration, nameof(ReceivedSignal.Duration)),
                    }
                );
    }
}
