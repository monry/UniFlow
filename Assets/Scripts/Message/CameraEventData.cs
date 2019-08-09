using UniFlow.Connector;

namespace UniFlow.Message
{
    public class CameraEventData
    {
        private CameraEventData(CameraEventType eventType)
        {
            EventType = eventType;
        }

        public CameraEventType EventType { get; }

        public static CameraEventData Create(CameraEventType eventType) =>
            new CameraEventData(eventType);
    }
}