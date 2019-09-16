using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace UniFlow.Editor
{
    public class SearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private FlowGraphView FlowGraphView { get; set; }

        private List<SearchTreeEntry> SearchTreeEntries { get; set; }

        private Texture2D DummyIcon { get; set; }

        public FlowPort FlowPort { get; set; }

        private IDictionary<string, SearchTreeGroupEntry> SearchTreeGroupEntries { get; } = new Dictionary<string, SearchTreeGroupEntry>();

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

            AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && x.IsSubclassOf(typeof(ConnectableBase)) && !x.IsAbstract && x.GetCustomAttributes(false).Any(y => y is AddComponentMenu))
                .Where(x => x.GetCustomAttributes(typeof(AddComponentMenu), false).Any())
                .Select(x => (menu: x.GetCustomAttributes(typeof(AddComponentMenu), false).OfType<AddComponentMenu>().First(), type: x))
                .OrderBy(x => x.menu.componentMenu)
                .ToList()
                .ForEach(
                    item =>
                    {
                        var (menu, type) = item;
                        var componentMenuEntries = menu
                            .componentMenu
                            .Split('/')
                            // Ignore first entry `UniFlow`
                            .Skip(1)
                            .ToArray();
                        var fullPath = new StringBuilder();
                        foreach (var (componentMenuEntry, index) in componentMenuEntries.Select((x, i) => (x, i)))
                        {
                            fullPath.Append($"/{componentMenuEntry}");
                            if (index == componentMenuEntries.Length - 1)
                            {
                                break;
                            }

                            if (SearchTreeGroupEntries.ContainsKey(fullPath.ToString()))
                            {
                                continue;
                            }

                            SearchTreeGroupEntries[fullPath.ToString()] = new SearchTreeGroupEntry(new GUIContent(componentMenuEntry)) {level = index + 1};
                            SearchTreeEntries.Add(SearchTreeGroupEntries[fullPath.ToString()]);
                        }

                        SearchTreeEntries.Add(new SearchTreeEntry(new GUIContent(componentMenuEntries.Last(), DummyIcon)) {level = componentMenuEntries.Length, userData = type});
                    }
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

            if (FlowPort != default)
            {
                FlowGraphView.AddEdge(FlowPort, (FlowPort) node.InputPort);
                if (FlowPort.node is FlowNode targetFlowNode && targetFlowNode.ConnectableInfo.Connectable is ConnectorBase targetConnector)
                {
                    targetConnector.TargetComponents = new List<ConnectableBase> {node.ConnectableInfo.Connectable as ConnectableBase};
                }
            }
            FlowGraphView.SetupActAsTrigger();

            return true;
        }
    }
}
