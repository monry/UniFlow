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

        private static IEnumerable<string> DirectoryOrder { get; } = new[]
        {
            "Controller",
            "Event",
            "ValueProvider",
            "ValueComparer",
            "Logic",
            "Receiver",
            "Misc",
        };

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
                .Where(x => x.IsClass && x.IsSubclassOf(typeof(ConnectorBase)) && !x.IsAbstract && x.GetCustomAttributes(false).Any(y => y is AddComponentMenu))
                .Where(x => x.GetCustomAttributes(typeof(AddComponentMenu), false).Any())
                .Select(x => (menu: x.GetCustomAttributes(typeof(AddComponentMenu), false).OfType<AddComponentMenu>().First(), type: x))
                .Select(x => (x.menu, x.type, entries: x.menu.componentMenu.Split('/').Skip(1).ToArray()))
                .Select(x => (x.menu, x.type, x.entries, directory: x.entries.Take(x.entries.Length - 1).Aggregate((a, b) => $"{a}/{b}")))
                .Select(x => (x.menu, x.type, x.entries, x.directory, directoryOrder: DirectoryOrder.Contains(x.directory) ? DirectoryOrder.Select((directory, index) => (directory, index)).First(y => y.directory == x.directory).index : int.MaxValue))
                .OrderBy(x => x.directoryOrder)
                .ThenBy(x => x.menu.componentOrder)
                .ToList()
                .ForEach(
                    item =>
                    {
                        var (_, type, componentMenuEntries, _, _) = item;
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
            if (!typeof(IConnector).IsAssignableFrom((Type) searchTreeEntry.userData))
            {
                return false;
            }

            var node = FlowGraphView.AddNode((Type) searchTreeEntry.userData, null, FlowGraphView.NormalizeMousePosition(context.screenMousePosition));

            if (FlowPort != default)
            {
                var edge = FlowGraphView.AddEdge(FlowPort, (FlowPort) node.InputPort);
                FlowGraphView.AddElement(edge);
                FlowGraphView.SetupActAsTrigger();
                if (FlowPort.node is FlowNode targetFlowNode && targetFlowNode.ConnectorInfo.Connector is ConnectorBase targetConnector)
                {
                    targetConnector.TargetComponents = new List<ConnectorBase> {node.ConnectorInfo.Connector as ConnectorBase};
                }
            }
            FlowGraphView.SetupActAsTrigger();

            return true;
        }
    }
}
