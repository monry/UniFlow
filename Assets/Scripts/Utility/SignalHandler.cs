using System;
using UniFlow.Signal;
using UniRx;

namespace UniFlow.Utility
{
    public static class SignalHandler<TSignal> where TSignal : ISignal
    {
        public static void Publish(TSignal signal)
        {
            MessageBroker.Default.Publish(signal);
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
