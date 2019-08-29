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

        [MenuItem("Window/UniFlow/Open UniFlow Graph %#u")]
        public static void Open()
        {
            var flowEditorWindow = GetWindow<FlowEditorWindow>();
            flowEditorWindow.titleContent = new GUIContent("UniFlow Graph");
        }

        private void OnEnable()
        {
            AssetReferences.Reload();

            var flowVisualElement = new FlowVisualElement
            {
                name = typeof(FlowVisualElement).Name,
            };
            rootVisualElement.Add(flowVisualElement);

            Repaint();
        }
    }
}