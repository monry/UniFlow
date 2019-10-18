using System;
using Zenject;

namespace UniFlow
{
    public class BoundarySignalCoordinator<TValue> : IBoundarySignalCoordinator<TValue>
    {
        [Inject] private SignalBus SignalBus { get; }

        void IBoundarySignalSender<TValue>.Send(TValue value)
        {
            SignalBus.Fire(value);
        }

        IObservable<TValue> IBoundarySignalReceiver<TValue>.OnReceiveAsObservable()
        {
            return SignalBus.GetStream<TValue>();
        }
    }
}
