using System;

namespace UniFlow
{
    public interface IEventPublisher : IEventConnectable
    {
        IObservable<EventMessage> OnPublishAsObservable();
        void Connect(IObservable<EventMessages> source);
    }
}