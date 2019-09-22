using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/CanvasGroupController", (int) ConnectorType.CanvasGroupController)]
    public class CanvasGroupController: ConnectorBase
    {
        [SerializeField] private CanvasGroupControlMethod canvasGroupControlMethod = CanvasGroupControlMethod.Activate;
        [SerializeField] private List<CanvasGroup> canvasGroups = default;

        [UsedImplicitly] private CanvasGroupControlMethod CanvasGroupControlMethod
        {
            get => canvasGroupControlMethod;
            set => canvasGroupControlMethod = value;
        }
        [UsedImplicitly] private IEnumerable<CanvasGroup> CanvasGroups
        {
            get => canvasGroups;
            set => canvasGroups = value.ToList();
        }

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            var count = HandleActivation();
            return Observable.Return(Message.Create(this, count));
        }

        private int HandleActivation()
        {
            var targets = CanvasGroups.Where(x => x.blocksRaycasts != (CanvasGroupControlMethod == CanvasGroupControlMethod.Activate)).ToList();
            var count = targets.Count;
            targets.ForEach(x => x.blocksRaycasts = CanvasGroupControlMethod == CanvasGroupControlMethod.Activate);
            return count;
        }

        public class Message : MessageBase<CanvasGroupController, int>, IValueHolder<int>
        {
            public static Message Create(CanvasGroupController sender, int count)
            {
                return Create<Message>(ConnectorType.CanvasGroupController, sender, count);
            }

            public int Value => Data;
        }
    }

    [PublicAPI]
    public enum CanvasGroupControlMethod
    {
        Activate,
        Deactivate,
    }
}
