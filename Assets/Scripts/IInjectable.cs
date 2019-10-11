using System;
using UniRx;

namespace UniFlow
{
    public interface IInjectable<in T>
    {
        void Inject(T value);
        IObservable<Unit> WaitForInjectAsObservable();
    }
}
