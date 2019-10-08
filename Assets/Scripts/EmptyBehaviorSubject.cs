using System;
using JetBrains.Annotations;
using UniRx;
using UniRx.InternalUtil;

namespace UniFlow
{
    [PublicAPI]
    internal sealed class EmptyBehaviorSubject<T> : ISubject<T>
    {
        private readonly object observerLock = new object();

        private bool isStopped;
        private bool isDisposed;
        private bool hasSetValue;
        private T lastValue;
        private Exception lastError;
        private IObserver<T> outObserver = EmptyObserver<T>.Instance;

        public T Value
        {
            get
            {
                ThrowIfDisposed();
                lastError?.Throw();
                return !hasSetValue ? default : lastValue;
            }
        }

        public bool HasObservers => !(outObserver is EmptyObserver<T>) && !isStopped && !isDisposed;

        public void OnCompleted()
        {
            IObserver<T> old;
            lock (observerLock)
            {
                ThrowIfDisposed();
                if (isStopped)
                {
                    return;
                }

                old = outObserver;
                outObserver = EmptyObserver<T>.Instance;
                isStopped = true;
            }

            old.OnCompleted();
        }

        public void OnError(Exception error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));

            IObserver<T> old;
            lock (observerLock)
            {
                ThrowIfDisposed();
                if (isStopped)
                {
                    return;
                }

                old = outObserver;
                outObserver = EmptyObserver<T>.Instance;
                isStopped = true;
                lastError = error;
            }

            old.OnError(error);
        }

        public void OnNext(T value)
        {
            IObserver<T> current;
            lock (observerLock)
            {
                if (isStopped)
                {
                    return;
                }

                lastValue = value;
                hasSetValue = true;
                current = outObserver;
            }

            current.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            var ex = default(Exception);
            var v = default(T);
            var subscription = default(Subscription);

            lock (observerLock)
            {
                ThrowIfDisposed();
                if (!isStopped)
                {
                    if (!hasSetValue)
                    {
                        return Disposable.Empty;
                    }

                    if (outObserver is ListObserver<T> listObserver)
                    {
                        outObserver = listObserver.Add(observer);
                    }
                    else
                    {
                        var current = outObserver;
                        outObserver = current is EmptyObserver<T> ? observer : new ListObserver<T>(new ImmutableList<IObserver<T>>(new[] { current, observer }));
                    }

                    v = lastValue;
                    subscription = new Subscription(this, observer);
                }
                else
                {
                    ex = lastError;
                }
            }

            if (subscription != null && hasSetValue)
            {
                observer.OnNext(v);
                return subscription;
            }

            if (ex != null)
            {
                observer.OnError(ex);
                return Disposable.Empty;
            }

            observer.OnCompleted();
            return Disposable.Empty;
        }

        public void Dispose()
        {
            lock (observerLock)
            {
                isDisposed = true;
                outObserver = DisposedObserver<T>.Instance;
                lastError = null;
                lastValue = default;
            }
        }

        private void ThrowIfDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("");
            }
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return false;
        }

        private class Subscription : IDisposable
        {
            private readonly object gate = new object();
            private EmptyBehaviorSubject<T> parent;
            private IObserver<T> unsubscribeTarget;

            public Subscription(EmptyBehaviorSubject<T> parent, IObserver<T> unsubscribeTarget)
            {
                this.parent = parent;
                this.unsubscribeTarget = unsubscribeTarget;
            }

            public void Dispose()
            {
                lock (gate)
                {
                    // ReSharper disable once InvertIf
                    if (parent != null)
                    {
                        lock (parent.observerLock)
                        {
                            if (parent.outObserver is ListObserver<T> listObserver)
                            {
                                parent.outObserver = listObserver.Remove(unsubscribeTarget);
                            }
                            else
                            {
                                parent.outObserver = EmptyObserver<T>.Instance;
                            }

                            unsubscribeTarget = null;
                            parent = null;
                        }
                    }
                }
            }
        }
    }

    public static class ExceptionExtensions
    {
        public static void Throw(this Exception exception)
        {
            throw exception;
        }
    }
    public class ListObserver<T> : IObserver<T>
    {
        private readonly ImmutableList<IObserver<T>> observers;

        public ListObserver(ImmutableList<IObserver<T>> observers)
        {
            this.observers = observers;
        }

        public void OnCompleted()
        {
            var targetObservers = observers.Data;
            foreach (var targetObserver in targetObservers)
            {
                targetObserver.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            var targetObservers = observers.Data;
            foreach (var targetObserver in targetObservers)
            {
                targetObserver.OnError(error);
            }
        }

        public void OnNext(T value)
        {
            var targetObservers = observers.Data;
            foreach (var targetObserver in targetObservers)
            {
                targetObserver.OnNext(value);
            }
        }

        internal IObserver<T> Add(IObserver<T> observer)
        {
            return new ListObserver<T>(observers.Add(observer));
        }

        internal IObserver<T> Remove(IObserver<T> observer)
        {
            var i = Array.IndexOf(observers.Data, observer);
            if (i < 0)
            {
                return this;
            }

            return observers.Data.Length == 2 ? observers.Data[1 - i] : new ListObserver<T>(observers.Remove(observer));
        }
    }

    public class EmptyObserver<T> : IObserver<T>
    {
        public static readonly EmptyObserver<T> Instance = new EmptyObserver<T>();

        private EmptyObserver()
        {
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(T value)
        {
        }
    }

    public class DisposedObserver<T> : IObserver<T>
    {
        public static readonly DisposedObserver<T> Instance = new DisposedObserver<T>();

        private DisposedObserver()
        {
        }

        public void OnCompleted()
        {
            throw new ObjectDisposedException("");
        }

        public void OnError(Exception error)
        {
            throw new ObjectDisposedException("");
        }

        public void OnNext(T value)
        {
            throw new ObjectDisposedException("");
        }
    }
}
