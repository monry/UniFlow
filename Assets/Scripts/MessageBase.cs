using UniFlow.Message;

namespace UniFlow
{
    public class MessageBase<TSender, TData> : IMessage<TSender, TData> where TSender : IConnector
    {
        public ConnectorType ConnectorType { get; private set; }
        public TSender Sender { get; private set; }
        public TData Data { get; private set; }

        protected static TResult Create<TResult>(ConnectorType connectorType, TSender sender, TData data) where TResult : MessageBase<TSender, TData>, new()
        {
            return new TResult
            {
                ConnectorType = connectorType,
                Sender = sender,
                Data = data,
            };
        }
    }

    public class MessageBase<TSender> : MessageBase<TSender, Empty> where TSender : IConnector
    {

        protected static TResult Create<TResult>(ConnectorType connectorType, TSender sender) where TResult : MessageBase<TSender>, new()
        {
            return Create<TResult>(connectorType, sender, Empty.Default);
        }
    }
}