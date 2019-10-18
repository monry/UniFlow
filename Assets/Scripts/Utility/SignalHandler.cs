using System;
using JetBrains.Annotations;
using UniFlow.Signal;
using UniRx;

namespace UniFlow.Utility
{
    [PublicAPI]
    public static class SignalHandler
    {
        public static void PublishString(string signalName)
        {
            SignalHandler<StringSignal>.Publish(new StringSignal(signalName));
        }

        public static void PublishString(string signalName, StringSignal.SignalParameter signalParameter)
        {
            SignalHandler<StringSignal>.Publish(new StringSignal(signalName, signalParameter));
        }

        public static IObservable<StringSignal> OnReceiveStringAsObservable(string signalName)
        {
            return SignalHandler<StringSignal>.OnReceiveAsObservable().Where(x => x.Name == signalName);
        }
    }

    [PublicAPI]
    public static class SignalHandler<TSignal> where TSignal : ISignal
    {
        public static void Publish(TSignal signal)
        {
            if (typeof(ScriptableObjectSignal).IsAssignableFrom(typeof(TSignal)))
            {
                MessageBroker.Default.Publish(signal as ScriptableObjectSignal);
            }
            else
            {
                MessageBroker.Default.Publish(signal);
            }
        }

        public static IObservable<TSignal> OnReceiveAsObservable()
        {
            return typeof(ScriptableObjectSignal).IsAssignableFrom(typeof(TSignal))
                ? MessageBroker.Default.Receive<ScriptableObjectSignal>().Where(x => x is TSignal).Select(x => (TSignal) (ISignal) x)
                : MessageBroker.Default.Receive<TSignal>();
        }

        public static IObservable<TSignal> OnReceiveAsObservable(TSignal signal)
        {
            return OnReceiveAsObservable().Where(x => !(x is IEquatableSignal<TSignal> valueSignal) || valueSignal.Equals(signal));
        }
    }
}
