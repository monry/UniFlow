using System;
using UniRx;

namespace UniFlow.Receiver
{
    public abstract class ObservableReceiverBase : ReceiverBase, IObservableReceiver
    {
        private ISubject<Unit> OnReceiveSubject { get; } = new Subject<Unit>();

        IObservable<Unit> IObservableReceiver.OnReceiveAsObservable()
        {
            return OnReceiveSubject;
        }

        public override void OnReceive()
        {
            OnReceiveSubject.OnNext(Unit.Default);
        }
    }
}
