using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniFlow.Editor
{
    public class FlowVisualElement : VisualElement
    {
        internal FlowGraphView FlowGraphView { get; private set; }
        private VisualElement Content { get; set; }

        public void Initialize()
        {
            styleSheets.Add(AssetReferences.Instance.FlowVisualElement);

            var toolbar = new IMGUIContainer(
                () =>
                {
                    GUILayout.BeginHorizontal(EditorStyles.toolbar);
                    if (GUILayout.Button("Load", EditorStyles.toolbarButton))
                    {
                        UniFlowSettings.instance.SelectedGameObject = Selection.activeGameObject;
                        if (Contains(Content))
                        {
                            Remove(Content);
                        }
                        Load();
                    }
                    if (GUILayout.Button("Load All in Scene", EditorStyles.toolbarButton))
                    {
                        UniFlowSettings.instance.SelectedGameObject = null;
                        if (Contains(Content))
                        {
                            Remove(Content);
                        }
                        Load();
                    }

                    GUILayout.Space(6);

                    if (GUILayout.Button($"Save {(UniFlowSettings.instance.IsPrefabMode ? "Prefab" : "Scenes")}", EditorStyles.toolbarButton))
                    {
                        if (UniFlowSettings.instance.IsPrefabMode)
                        {
                            PrefabUtility.SavePrefabAsset(UniFlowSettings.instance.SelectedGameObject);
                        }
                        else if (UniFlowSettings.instance.SelectedGameObject != default)
                        {
                            EditorSceneManager.SaveScene(UniFlowSettings.instance.SelectedGameObject.scene);
                        }
                    }

                    GUILayout.Space(6);

                    if (GUILayout.Button("Reset Viewport", EditorStyles.toolbarButton))
                    {
                        UniFlowSettings.instance.LatestPosition = Vector3.zero;
                        UniFlowSettings.instance.LatestScale = Vector3.one;
                        Remove(Content);
                        Load();
                    }
                    if (GUILayout.Button("Auto Layout", EditorStyles.toolbarButton))
                    {
                        FlowGraphView.Relocation(true);
                    }

                    if (UniFlowSettings.instance.SelectedGameObject != default)
                    {
                        GUILayout.Space(6);

                        // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                        if (UniFlowSettings.instance.IsPrefabMode)
                        {
                            GUILayout.Label($"[Prefab] {CreateGameObjectTreeString(UniFlowSettings.instance.SelectedGameObject.transform)}");
                        }
                        else
                        {
                            GUILayout.Label($"[{UniFlowSettings.instance.SelectedGameObject.scene.name}] {CreateGameObjectTreeString(UniFlowSettings.instance.SelectedGameObject.transform)}");
                        }
                    }

                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            );
            Add(toolbar);

            Load();
        }

        private static string CreateGameObjectTreeString(Transform transform)
        {
            if (!transform.gameObject.scene.IsValid())
            {
                return AssetDatabase.GetAssetPath(transform.gameObject);
            }

            var parent = transform.parent;
            return parent == default ? transform.name : $"{CreateGameObjectTreeString(parent)} > {transform.name}";
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
