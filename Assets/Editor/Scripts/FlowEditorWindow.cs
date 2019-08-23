using UnityEditor;
using UnityEngine;

namespace UniFlow.Editor
{
    public class FlowEditorWindow : EditorWindow
    {
        private static FlowEditorWindow window = default;

        private static FlowEditorWindow Window
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

        [MenuItem("Window/UniFlow/Open UniFlow Window %#u")]
        public static void Open()
        {
            Window.Load();

            var originalCallback = Selection.selectionChanged;
            Selection.selectionChanged = () =>
            {
                originalCallback?.Invoke();
                Window.Load();
            };
        }

        [MenuItem("Window/UniFlow/Reload UniFlow Window %#&u")]
        public static void Reload()
        {
            Window.Load();
        }

        private void Load()
        {
            AssetReferences.Reload();
            rootVisualElement.Clear();
            var flowVisualElement = new FlowVisualElement
            {
                name = typeof(FlowVisualElement).Name,
            };
            rootVisualElement.Add(flowVisualElement);
            titleContent = new GUIContent("UniFlow Graph");
            Repaint();
        }
    }
}