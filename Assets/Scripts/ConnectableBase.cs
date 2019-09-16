using UnityEngine;

namespace UniFlow
{
    public abstract class ConnectableBase : MonoBehaviour, IConnectable
    {
        protected virtual void Start()
        {
            CollectSuppliedValues();
        }

        protected virtual void CollectSuppliedValues()
        {
            // Do nothing
        }

#if UNITY_EDITOR
        [SerializeField] [HideInInspector] private Vector2 flowGraphNodePosition = default;
        public Vector2 FlowGraphNodePosition
        {
            get => flowGraphNodePosition;
            set => flowGraphNodePosition = value;
        }
#endif
    }
}