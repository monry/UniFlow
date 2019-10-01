using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/MoveParentTransform", (int) ConnectorType.MoveParentTransform)]
    public class MoveParentTransform : ConnectorBase
    {
        // ReSharper disable once InconsistentNaming
        [SerializeField] private Transform targetTransform = default;
        [SerializeField] private Transform parentTransform = default;
        [SerializeField] private bool worldPositionStays = true;

        [ValueReceiver] public Transform TargetTransform
        {
            get => targetTransform != default
                ? targetTransform
                : targetTransform = transform;
            set => targetTransform = value;
        }
        [ValueReceiver] public Transform ParentTransform
        {
            get => parentTransform;
            set => parentTransform = value;
        }
        [ValueReceiver] public bool WorldPositionStays
        {
            get => worldPositionStays;
            set => worldPositionStays = value;
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            TargetTransform.SetParent(ParentTransform, WorldPositionStays);
            return Observable.ReturnUnit();
        }
    }
}
