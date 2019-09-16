namespace UniFlow
{
    public interface IMessage
    {
        ConnectorType ConnectorType { get; }
    }

    public interface IMessage<out TSender> : IMessage
    {
        TSender Sender { get; }
    }

    public interface IMessage<out TSender, out TData> : IMessage<TSender>
    {
        TData Data { get; }
    }

    public static class MessageExtensions
    {
        public static bool Is<TMessage>(this IMessage message) where TMessage : IMessage
        {
            return message is TMessage;
        }

        public static TMessage As<TMessage>(this IMessage message) where TMessage : class, IMessage
        {
            return message as TMessage;
        }
    }
}