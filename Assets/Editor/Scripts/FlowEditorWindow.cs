using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace UniFlow.Editor
{
    internal class FlowGraphParameters : ScriptableSingleton<FlowGraphParameters>
    {
        [SerializeField] private Vector3 latestPosition = Vector3.zero;
        [SerializeField] private Vector3 latestScale = Vector3.one;

        internal Vector3 LatestPosition
        {
            get => latestPosition;
            set => latestPosition = value;
        }

        internal Vector3 LatestScale
        {
            get => latestScale;
            set => latestScale = value;
        }
    }

    public class FlowEditorWindow : EditorWindow
    {
        private static FlowEditorWindow window = default;

        internal static FlowEditorWindow Window
        {
            get
            {
                if (window == default)
                {
                    window = GetWindow<FlowEditorWindow>();
                }

                return window;
            }
        }

        [SerializeField] private List<ConnectableInfo> connectableInfoList = new List<ConnectableInfo>();

        public IList<ConnectableInfo> ConnectableInfoList => connectableInfoList;

        [MenuItem("Window/UniFlow/Open UniFlow Graph %#u")]
        public static void Open()
        {
            window = GetWindow<FlowEditorWindow>();
            window.titleContent = new GUIContent("UniFlow Graph");
        }

        [SerializeField] [UsedImplicitly] private int counter;

        public void ForceRegisterUndo()
        {
            counter++;
        }

        private void Reload()
        {
            AssetReferences.Reload();

            rootVisualElement.Clear();
            connectableInfoList = new List<ConnectableInfo>();
            var flowVisualElement = new FlowVisualElement
            {
                name = typeof(FlowVisualElement).Name,
            };
            rootVisualElement.Add(flowVisualElement);

            Repaint();
        }

        private void OnEnable()
        {
            Undo.undoRedoPerformed += Reload;
            Reload();
        }

        private void OnDisable()
        {
            // ReSharper disable once DelegateSubtraction
            Undo.undoRedoPerformed -= Reload;
        }

        private void OnDestroy()
        {
            window = null;
        }
    }
}