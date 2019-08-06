using UniRx;

namespace EventConnector.Connector
{
    public class Empty : EventConnector
    {
        protected override void Connect(EventMessages eventMessages)
        {
            Observable
                .EveryEndOfFrame()
                .Take(1)
                .SubscribeWithState(
                    eventMessages,
                    (_, em) => OnConnect(em.Append(EventMessage.Create(EventType.Empty, this)))
                );
        }
    }
}