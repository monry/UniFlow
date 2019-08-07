using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace EventConnector
{
    public abstract class EventConnector : MonoBehaviour, IEventConnector, IEventConnectable
    {
        [SerializeField]
        [Tooltip("Specify instances of IEventConnector directly")]
        private List<EventConnector> sourceConnectorInstances = default;
        [SerializeField]
        [Tooltip("Specify identifiers of IEventConnector that resolve from Zenject.DiContainer")]
        private List<string> sourceConnectorIds = default;
        [SerializeField]
        [Tooltip("Check if you want to act as Receiver")]
        private bool actAsReceiver = false;

        private IEnumerable<IEventConnector> SourceConnectors =>
            new List<IEventConnector>()
                .Concat(sourceConnectorInstances ?? new List<EventConnector>())
                .Concat((sourceConnectorIds ?? new List<string>()).SelectMany(Container.ResolveIdAll<IEventConnector>));
        private bool ActAsReceiver => actAsReceiver;
        private ISubject<EventMessages> Subject { get; } = new Subject<EventMessages>();

        [Inject] private DiContainer Container { get; }

        protected virtual IEnumerator Start()
        {
            if (!MainThreadDispatcher.IsInitialized)
            {
                MainThreadDispatcher.Initialize();

                while (!MainThreadDispatcher.IsInitialized)
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            if (ActAsReceiver)
            {
                ((IEventConnector) this)
                    .ConnectAsObservable()
                    .Subscribe()
                    .AddTo(this);
            }
        }

        public abstract IObservable<EventMessage> FooAsObservable();

        IObservable<EventMessages> IEventConnector.ConnectAsObservable()
        {
            // SourceConnector が無いなら new する
            GenerateSourceObservable().Subscribe(x => ((IObserver<EventMessages>) this).OnNext(x));
            return this;
        }

        protected abstract void Connect(EventMessages eventMessages);

        protected void OnConnect(EventMessages eventMessages)
        {
            Subject.OnNext(eventMessages);
        }

        bool IEventConnectable.HasSourceConnectors() =>
            SourceConnectors.Any();

        private IObservable<EventMessages> GenerateSourceObservable() =>
            ((IEventConnectable) this).HasSourceConnectors()
                ? SourceConnectors.Select(x => x.ConnectAsObservable()).Merge()
                : Observable.Defer(() => Observable.Return(EventMessages.Create()));

        void IObserver<EventMessages>.OnCompleted()
        {
            Subject.OnCompleted();
        }

        void IObserver<EventMessages>.OnError(Exception error)
        {
            Subject.OnError(error);
        }

        void IObserver<EventMessages>.OnNext(EventMessages value)
        {
            Connect(value);
        }

        IDisposable IObservable<EventMessages>.Subscribe(IObserver<EventMessages> observer)
        {
            return Subject.Subscribe(observer);
        }
    }
}