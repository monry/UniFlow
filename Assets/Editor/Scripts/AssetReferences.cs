using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniFlow.Editor
{
    [CreateAssetMenu(menuName = "UniFlow/Editor AssetReferences", fileName = "AssetReferences.asset")]
    public class AssetReferences : ScriptableObject
    {
        [SerializeField] private StyleSheet flowVisualElement = default;
        [SerializeField] private StyleSheet flowGraphView = default;
        [SerializeField] private StyleSheet flowNode = default;

        public StyleSheet FlowVisualElement => flowVisualElement;
        public StyleSheet FlowGraphView => flowGraphView;
        public StyleSheet FlowNode => flowNode;

        private static AssetReferences instance = default;
        public static AssetReferences Instance => instance ? instance : instance = Load();

        public static void Reload()
        {
            instance = Load();
        }

        private static AssetReferences Load()
        {
            return AssetDatabase
                .FindAssets("t:UniFlow.Editor.AssetReferences")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<AssetReferences>)
                .FirstOrDefault();
        }
    }
}