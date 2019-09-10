namespace UniFlow
{
    public interface IEventMessage
    {
        ConnectorType ConnectorType { get; }
    }

    public interface IEventMessage<out TSender> : IEventMessage
    {
        TSender Sender { get; }
    }
}