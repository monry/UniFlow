using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    public abstract class Base<TValue, TOperator> : ConnectorBase
        where TOperator : Enum
    {
        [SerializeField] private TValue expect = default;
        [SerializeField] private TOperator @operator = default;

        [UsedImplicitly] public TValue Value
        {
            get => expect;
            set => expect = value;
        }
        [UsedImplicitly] public TOperator Operator
        {
            get => @operator;
            set => @operator = value;
        }

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            if (latestMessage.Is<IValueHolder<TValue>>() && Compare(latestMessage.As<IValueHolder<TValue>>().Value))
            {
                return Observable.Return(Message.Create(this));
            }

            return Observable.Empty<IMessage>();
        }

        protected abstract bool Compare(TValue compareValue);

        private void OnDestroy()
        {
            Disposable.Dispose();
        }

        public class Message : MessageBase<Base<TValue, TOperator>>
        {
            public static Message Create(Base<TValue, TOperator> sender)
            {
                return Create<Message>(ConnectorType.ValueComparerEnum, sender);
            }
        }
    }
}
