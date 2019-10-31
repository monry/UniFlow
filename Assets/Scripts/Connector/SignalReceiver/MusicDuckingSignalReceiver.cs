using System.Collections.Generic;
using System.Linq;
using UniFlow.Signal;
using UnityEngine;

namespace UniFlow.Connector.SignalReceiver
{
    [AddComponentMenu("UniFlow/SignalReceiver/MusicDuckingSignalReceiver", (int) ConnectorType.MusicDuckingSignalReceiver)]
    public class MusicDuckingSignalReceiver : SignalReceiverBase<MusicDuckingSignal>
    {
        protected override MusicDuckingSignal GetSignal() =>
            MusicDuckingSignal.Create();

        public override IEnumerable<IComposableMessageAnnotation> GetMessageComposableAnnotations() =>
            base.GetMessageComposableAnnotations()
                .Concat(
                    new[]
                    {
                        ComposableMessageAnnotationFactory.Create(() => ReceivedSignal.Volume, nameof(ReceivedSignal.Volume)),
                        ComposableMessageAnnotationFactory.Create(() => ReceivedSignal.Duration, nameof(ReceivedSignal.Duration)),
                    }
                );
    }
}
