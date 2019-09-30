using System;
using UniRx;

namespace UniFlow.Connector.ValueProvider
{
    public abstract class Base<TValue> : ConnectorBase
    {
        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            return Observable.Return(Message.Create(this, Provide()));
        }

        protected abstract TValue Provide();

        private void OnDestroy()
        {
            Disposable.Dispose();
        }

        public class Message : MessageBase<Base<TValue>, TValue>, IValueHolder<TValue>
        {
            public static Message Create(Base<TValue> sender, TValue value)
            {
                return Create<Message>(ConnectorType.ValueProvider, sender, value);
            }

            public TValue Value => Data;
        }
    }
}
