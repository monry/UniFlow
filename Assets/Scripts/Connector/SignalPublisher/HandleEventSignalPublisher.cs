using System.Collections.Generic;
using UniFlow.Signal;
using UnityEngine;

namespace UniFlow.Connector.SignalPublisher
{
    [AddComponentMenu("UniFlow/SignalPublisher/HandleEventSignalPublisher", (int) ConnectorType.HandleEventSignalPublisher)]
    public class HandleEventSignalPublisher : SignalPublisherBase<HandleEventSignal>, IMessageCollectable
    {
        [SerializeField] private HandleEventType handleEventType = default;
        private HandleEventType HandleEventType
        {
            get => handleEventType;
            set => handleEventType = value;
        }

        [SerializeField] private HandleEventTypeCollector handleEventTypeCollector = new HandleEventTypeCollector();
        private HandleEventTypeCollector HandleEventTypeCollector => handleEventTypeCollector;

        protected override HandleEventSignal GetSignal() =>
            HandleEventSignal.Create(HandleEventType);

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(HandleEventTypeCollector, x => HandleEventType = x, nameof(HandleEventType)),
            };
    }
}
