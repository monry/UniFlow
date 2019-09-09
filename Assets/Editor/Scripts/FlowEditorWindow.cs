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
        internal static bool IsPrefabMode => Selection.activeGameObject != default && !Selection.activeGameObject.scene.IsValid();
    }

    public class FlowEditorWindow : EditorWindow
    {
        internal static FlowEditorWindow Window { get; private set; } = default;

        [SerializeField] private List<ConnectableInfo> connectableInfoList = new List<ConnectableInfo>();
        [SerializeField] [UsedImplicitly] private int counter = default;

        public IList<ConnectableInfo> ConnectableInfoList => connectableInfoList;

        [MenuItem("Window/UniFlow/Open UniFlow Graph")]
        public static void Open()
        {
            GetWindow<FlowEditorWindow>();
            Window.titleContent = new GUIContent("UniFlow Graph");
        }

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
            flowVisualElement.Initialize();
            rootVisualElement.Add(flowVisualElement);

            Repaint();
        }

        private void OnEnable()
        {
            Window = this;
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
            Window = null;
        }
    }
}