using System.Collections.Generic;
using System.Linq;
using UniFlow.Signal;
using UnityEngine;

namespace UniFlow.Connector.SignalReceiver
{
    [AddComponentMenu("UniFlow/SignalReceiver/MusicControlSignalReceiver", (int) ConnectorType.MusicControlSignalReceiver)]
    public class MusicControlSignalReceiver : SignalReceiverBase<MusicControlSignal>
    {
        protected override MusicControlSignal GetSignal() =>
            MusicControlSignal.Create();

        public override IEnumerable<IComposableMessageAnnotation> GetMessageComposableAnnotations() =>
            base.GetMessageComposableAnnotations()
                .Concat(
                    new[]
                    {
                        ComposableMessageAnnotationFactory.Create(() => ReceivedSignal.AudioControlMethod, nameof(ReceivedSignal.AudioControlMethod)),
                        ComposableMessageAnnotationFactory.Create(() => ReceivedSignal.AudioClip, nameof(ReceivedSignal.AudioClip)),
                        ComposableMessageAnnotationFactory.Create(() => ReceivedSignal.Loop, nameof(ReceivedSignal.Loop)),
                        ComposableMessageAnnotationFactory.Create(() => ReceivedSignal.RewindSameClip, nameof(ReceivedSignal.RewindSameClip)),
                    }
                );
    }
}
