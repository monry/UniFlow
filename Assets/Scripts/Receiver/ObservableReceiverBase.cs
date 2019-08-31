using System;
using UniRx;

namespace UniFlow.Receiver
{
    public abstract class ObservableReceiverBase : ReceiverBase, IObservableReceiver
    {
        private ISubject<EventMessages> OnReceiveSubject { get; } = new Subject<EventMessages>();

        IObservable<EventMessages> IObservableReceiver.OnReceiveAsObservable()
        {
            return OnReceiveSubject;
        }

        public override void OnReceive(EventMessages eventMessages)
        {
            OnReceiveSubject.OnNext(eventMessages);
        }
    }
}