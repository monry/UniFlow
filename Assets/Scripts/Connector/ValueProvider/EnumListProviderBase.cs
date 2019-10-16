using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace UniFlow.Connector.ValueProvider
{
    public abstract class EnumListProviderBase<TEnum> : ConnectorBase, IMessageComposable where TEnum : Enum
    {
        public override IObservable<Message> OnConnectAsObservable()
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Where(Filter)
                .ToList()
                .ToObservable()
                .AsMessageObservable(this, typeof(TEnum).Name);
        }

        protected virtual bool Filter(TEnum value)
        {
            return true;
        }

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                // Will compose parameter in OnConnectAsObservable()
                new ComposableMessageAnnotation<TEnum>(null),
            };
    }
}
