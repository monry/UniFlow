using System;
using UniRx;

namespace UniFlow.Receiver
{
    public abstract class ObservableReceiverBase : ReceiverBase, IObservableReceiver
    {
        private ISubject<Messages> OnReceiveSubject { get; } = new Subject<Messages>();

        IObservable<Messages> IObservableReceiver.OnReceiveAsObservable()
        {
            return OnReceiveSubject;
        }

        public override void OnReceive(Messages messages)
        {
            OnReceiveSubject.OnNext(messages);
        }
    }
}