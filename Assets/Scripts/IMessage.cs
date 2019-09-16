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

    public static class MessageExtensions
    {
        public static bool Is<TSender>(this IMessage message) where TSender : IMessage
        {
            return message is TSender;
        }

        public static TSender As<TSender>(this IMessage message) where TSender : class, IMessage
        {
            return message as TSender;
        }
    }
}