using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniFlow.Editor
{
    public class FlowGraphView : GraphView
    {
        public FlowGraphView()
        {
            styleSheets.Add(AssetReferences.Instance.FlowGraphView);
            SetupZoom(0.05f, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());

            var flowGridBackground = new FlowGridBackground
            {
                name = typeof(FlowGridBackground).Name,
            };
            Insert(0, flowGridBackground);
        }

        private IDictionary<IConnectable, IList<IConnectable>> DestinationConnectors { get; } = new Dictionary<IConnectable, IList<IConnectable>>();
        private IDictionary<IConnectable, IList<IConnectable>> SourceConnectors { get; } = new Dictionary<IConnectable, IList<IConnectable>>();
        private IDictionary<IConnectable, FlowNode> RenderedNodes { get; } = new Dictionary<IConnectable, FlowNode>();
        private IDictionary<IConnectable, Port> OutputPorts { get; } = new Dictionary<IConnectable, Port>();
        private IDictionary<IConnectable, Port> InputPorts { get; } = new Dictionary<IConnectable, Port>();

        public void Initialize()
        {
            var activeGameObject = Selection.activeGameObject;
            if (activeGameObject == default)
            {
                return;
            }

            var connectables = activeGameObject.scene.IsValid()
                ? activeGameObject.scene.GetRootGameObjects().SelectMany(x => x.GetComponentsInChildren<IConnectable>()).ToArray()
                : activeGameObject.GetComponentsInChildren<IConnectable>().ToArray();

            foreach (var connector in connectables.OfType<ConnectorBase>())
            {
                if (!DestinationConnectors.ContainsKey(connector))
                {
                    DestinationConnectors[connector] = new List<IConnectable>();
                }

                foreach (var targetConnector in connector.TargetComponents)
                {
                    if (!SourceConnectors.ContainsKey(targetConnector))
                    {
                        SourceConnectors[targetConnector] = new List<IConnectable>();
                    }

                    DestinationConnectors[connector].Add(targetConnector);
                    SourceConnectors[targetConnector].Add(connector);
                }
            }

            var rootConnectables = connectables.Where(x => !SourceConnectors.ContainsKey(x)).ToArray();
            if (!rootConnectables.Any() && connectables.Any())
            {
                rootConnectables = new[] {connectables.First()};
            }

            foreach (var (rootConnectable, index) in rootConnectables.Select((v, i) => (v, i)))
            {
                CreateNode(rootConnectable, 0, index * 400);
            }

            CreateEdges();
        }

        private void CreateNode(IConnectable connectable, int x, int y)
        {
            var node = new FlowNode
            {
                title = connectable.GetType().Name,
            };
            node.SetPosition(new Rect(x, y, 200, 200));

            if (DestinationConnectors.ContainsKey(connectable))
            {
                foreach (var (targetConnectable, index) in DestinationConnectors[connectable].Select((v, i) => (v, i)))
                {
                    if (!RenderedNodes.ContainsKey(targetConnectable))
                    {
                        CreateNode(targetConnectable, x + 300, y + index * 400);
                    }
                }
            }

            RenderedNodes[connectable] = node;
            AddElement(node);
        }

        private void CreateEdges()
        {
            foreach (var (connectable, targetConnectables) in DestinationConnectors.Select(x => (x.Key, x.Value)))
            {
                foreach (var targetConnectable in targetConnectables)
                {
                    if (!RenderedNodes.ContainsKey(connectable) || !RenderedNodes.ContainsKey(targetConnectable))
                    {
                        continue;
                    }

                    if (!OutputPorts.ContainsKey(connectable))
                    {
                        OutputPorts[connectable] = Port.Create<FlowEdge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(FlowPort));
                        OutputPorts[connectable].portName = "Out";
                        RenderedNodes[connectable].outputContainer.Add(OutputPorts[connectable]);
                    }

                    if (!InputPorts.ContainsKey(targetConnectable))
                    {
                        InputPorts[targetConnectable] = Port.Create<FlowEdge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(FlowPort));
                        InputPorts[targetConnectable].portName = "In";
                        RenderedNodes[targetConnectable].inputContainer.Add(InputPorts[targetConnectable]);
                    }

                    AddElement(
                        new FlowEdge
                        {
                            output = OutputPorts[connectable],
                            input = InputPorts[targetConnectable],
                        }
                    );
                }
            }
        }
    }
}