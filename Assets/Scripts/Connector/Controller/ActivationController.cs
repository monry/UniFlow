using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/ActivationController", (int) ConnectorType.LoadScene)]
    public class ActivationController : ConnectorBase
    {
        [SerializeField] private List<GameObject> targetGameObjects = default;
        [SerializeField] private List<MonoBehaviour> targetMonoBehaviours = default;
        [SerializeField] private bool activated = default;

        [UsedImplicitly] public IList<GameObject> GameObjects
        {
            get => targetGameObjects;
            set => targetGameObjects = value.ToList();
        }
        [UsedImplicitly] public IList<MonoBehaviour> MonoBehaviours
        {
            get => targetMonoBehaviours;
            set => targetMonoBehaviours = value.ToList();
        }
        [ValueReceiver] public bool Activated
        {
            get => activated;
            set => activated = value;
        }
        [ValueReceiver] public GameObject TargetGameObject
        {
            get => null;
            set => GameObjects.Add(value);
        }
        [ValueReceiver] public MonoBehaviour TargetMonoBehaviour
        {
            get => null;
            set => MonoBehaviours.Add(value);
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            var gameObjects = GameObjects.Where(x => x.activeSelf != Activated).ToList();
            gameObjects.ForEach(x => x.SetActive(Activated));
            var monoBehaviours = MonoBehaviours.Where(x => x.enabled != Activated).ToList();
            monoBehaviours.ForEach(x => x.enabled = Activated);

            return Observable.ReturnUnit();
        }
    }
}
