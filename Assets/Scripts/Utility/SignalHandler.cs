using System;
using JetBrains.Annotations;
using UniRx;
using Zenject;

namespace UniFlow.Utility
{
    public interface ISignalPublisher<in TSignal>
    {
        void Publish(TSignal signal);
    }

    public interface ISignalReceiver<out TSignal>
    {
        IObservable<TSignal> OnReceiveAsObservable();
    }

    public interface ISignalHandler<TSignal> : ISignalPublisher<TSignal>, ISignalReceiver<TSignal>
    {
    }

    [PublicAPI]
    public class SignalHandler<TSignal> : ISignalHandler<TSignal>
    {
        [Inject] private SignalBus SignalBus { get; }

        public void Publish(TSignal signal)
        {
            SignalBus.Fire(signal);
        }

        public IObservable<TSignal> OnReceiveAsObservable()
        {
            return SignalBus.GetStream<TSignal>();
        }
    }

    public static class SignalHandlerExtensions
    {
        public static IObservable<TSignal> OnReceiveAsObservable<TSignal>(this ISignalReceiver<TSignal> signalReceiver, TSignal signal)
        {
            return signalReceiver.OnReceiveAsObservable().Where(x => Equals(x, signal));
        }
    }
}
