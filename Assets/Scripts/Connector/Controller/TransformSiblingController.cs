using System;
using UniFlow.Utility;
using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/TransformSiblingController", (int) ConnectorType.TransformSiblingController)]
    public class TransformSiblingController : ConnectorBase
    {
        [SerializeField] private Transform targetTransform = default;
        [SerializeField] private TransformSiblingControlMethod transformSiblingControlMethod = default;
        [SerializeField] private int index = default;

        [ValueReceiver] public Transform TargetTransform
        {
            get => targetTransform != default
                ? targetTransform
                : targetTransform = transform;
            set => targetTransform = value;
        }
        private TransformSiblingControlMethod TransformSiblingControlMethod => transformSiblingControlMethod;
        [ValueReceiver] public int Index
        {
            get => index;
            set => index = value;
        }

        public override IObservable<Message> OnConnectAsObservable()
        {
            switch (TransformSiblingControlMethod)
            {
                case TransformSiblingControlMethod.SetIndex:
                    TargetTransform.SetSiblingIndex(Index);
                    break;
                case TransformSiblingControlMethod.AsFirst:
                    TargetTransform.SetAsFirstSibling();
                    break;
                case TransformSiblingControlMethod.AsLast:
                    TargetTransform.SetAsLastSibling();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return ObservableFactory.ReturnMessage(this);
        }
    }

    public enum TransformSiblingControlMethod
    {
        SetIndex,
        AsFirst,
        AsLast,
    }
}
