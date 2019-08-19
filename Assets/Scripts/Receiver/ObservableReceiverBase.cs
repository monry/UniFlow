using System;
using UniRx;

namespace UniFlow.Receiver
{
    public abstract class ObservableReceiverBase : ReceiverBase
    {
        private ISubject<EventMessages> OnReceiveSubject { get; } = new Subject<EventMessages>();

        protected IObservable<EventMessages> OnReceiveAsObservable()
        {
            return OnReceiveSubject;
        }

        public override void OnReceive(EventMessages eventMessages)
        {
            OnReceiveSubject.OnNext(eventMessages);
        }
    }
}