using System;
using UniRx;

namespace UniFlow
{
    public static class Logger
    {
        public static bool IsEnabled { get; private set; }

        private static ISubject<IConnector> ConnectorSubject { get; } = new Subject<IConnector>();

        public static void Activate()
        {
            IsEnabled = true;
        }

        public static void Deactivate()
        {
            IsEnabled = false;
        }

        public static void Log(IConnector connector)
        {
            ConnectorSubject.OnNext(connector);
        }

        public static IObservable<IConnector> OnMessageAsObservable()
        {
            return ConnectorSubject;
        }
    }
}
