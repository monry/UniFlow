using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniFlow.Editor
{
    public class SearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private FlowGraphView FlowGraphView { get; set; }

        private List<SearchTreeEntry> SearchTreeEntries { get; set; }

        private Texture2D DummyIcon { get; set; }

        public FlowPort FlowPort { get; set; }

        public void Initialize(FlowGraphView flowGraphView)
        {
            FlowGraphView = flowGraphView;

            DummyIcon = new Texture2D(1, 1);
            DummyIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            DummyIcon.Apply();

            SearchTreeEntries = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Add Node"))
            };
            SearchTreeEntries
                .AddRange(
                    AppDomain
                        .CurrentDomain
                        .GetAssemblies()
                        .SelectMany(x => x.GetTypes())
                        .Where(x => x.IsClass && x.IsSubclassOf(typeof(ConnectableBase)) && !x.IsAbstract && x.GetCustomAttributes(false).Any(y => y is AddComponentMenu))
                        .OrderBy(x => ((AddComponentMenu) x.GetCustomAttributes(false).FirstOrDefault())?.componentOrder)
                        .Select(x => new SearchTreeEntry(new GUIContent(x.Name, DummyIcon)) {level = 1, userData = x})
                        .ToList()
                );
        }

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            return SearchTreeEntries;
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (!typeof(IConnectable).IsAssignableFrom((Type) searchTreeEntry.userData))
            {
                return false;
            }

            var node = FlowGraphView.AddNode((Type) searchTreeEntry.userData, null, FlowGraphView.NormalizeMousePosition(context.screenMousePosition));
            node.Query<ObjectField>().Build().First().value = Selection.activeGameObject;

            if (FlowPort != default)
            {
                FlowGraphView.AddEdge(FlowPort, (FlowPort) node.InputPort);
            }

            return true;
        }
    }
}