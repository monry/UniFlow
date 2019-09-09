using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniFlow.Editor
{
    public class FlowVisualElement : VisualElement
    {
        private FlowGraphView FlowGraphView { get; set; }
        private VisualElement Content { get; set; }

        public void Initialize()
        {
            styleSheets.Add(AssetReferences.Instance.FlowVisualElement);

            Selection.selectionChanged += () =>
            {
                if (FlowEditorWindow.Window != default)
                {
                    Remove(Content);
                    Load();
                }
            };

            var toolbar = new IMGUIContainer(
                () =>
                {
                    GUILayout.BeginHorizontal(EditorStyles.toolbar);
                    if (GUILayout.Button($"Save {(FlowGraphParameters.IsPrefabMode ? "Prefab" : "Scenes")}", EditorStyles.toolbarButton))
                    {
                        if (FlowGraphParameters.IsPrefabMode)
                        {
                            PrefabUtility.SavePrefabAsset(Selection.activeGameObject);
                        }
                        else if (Selection.activeGameObject != default)
                        {
                            EditorSceneManager.SaveScene(Selection.activeGameObject.scene);
                        }
                    }

                    GUILayout.Space(6);

                    if (GUILayout.Button("Reset Viewport", EditorStyles.toolbarButton))
                    {
                        FlowGraphParameters.instance.LatestPosition = Vector3.zero;
                        FlowGraphParameters.instance.LatestScale = Vector3.one;
                        Remove(Content);
                        Load();
                    }
                    if (GUILayout.Button("Auto Layout", EditorStyles.toolbarButton))
                    {
                        FlowGraphView.Relocation(true);
                    }

                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            );
            Add(toolbar);

            Load();
        }

        private void Load()
        {
            FlowGraphView = new FlowGraphView
            {
                name = typeof(FlowGraphView).Name,
            };
            FlowGraphView.Initialize();
            Content = new VisualElement
            {
                name = "Content",
            };
            Content.Add(FlowGraphView);
            Add(Content);
        }
    }
}