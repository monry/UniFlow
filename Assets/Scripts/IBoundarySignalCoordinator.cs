using System;
using UniRx;

namespace UniFlow
{
    public interface IBoundarySignalSender<in TValue>
    {
        void Send(TValue value);
    }

    public interface IBoundarySignalReceiver<out TValue>
    {
        IObservable<TValue> OnReceiveAsObservable();
    }

    public interface IBoundarySignalCoordinator<TValue> : IBoundarySignalSender<TValue>, IBoundarySignalReceiver<TValue>
    {
    }

    public static class BoundaryCoordinatorExtensions
    {
        public static IObservable<TValue> OnReceiveAsObservable<TValue>(this IBoundarySignalReceiver<TValue> messenger, TValue value)
        {
            return messenger.OnReceiveAsObservable().Where(x => Equals(x, value));
        }
    }
}
