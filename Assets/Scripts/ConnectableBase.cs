using UnityEngine;

namespace UniFlow
{
    public abstract class ConnectableBase : MonoBehaviour, IConnectable
    {
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