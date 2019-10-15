using System;
using UniRx;

namespace UniFlow.Utility
{
    public static class ObservableFactory
    {
        public static IObservable<Message> ReturnMessage(IConnector connector)
        {
            return Observable.Return(connector.CreateMessage());
        }

        public static IObservable<Message> ReturnMessage<T>(IConnector connector, string key, T parameter)
        {
            return Observable.Return(connector.CreateMessage(parameter, key));
        }

        public static IObservable<Message> EmptyMessage()
        {
            return Observable.Empty<Message>();
        }
    }
}
