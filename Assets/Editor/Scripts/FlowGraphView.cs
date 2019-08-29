using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UniFlow.Editor
{
    public class FlowGraphView : GraphView
    {
        private IDictionary<IConnectable, IList<IConnectable>> DestinationConnectors { get; } = new Dictionary<IConnectable, IList<IConnectable>>();
        private IDictionary<IConnectable, IList<IConnectable>> SourceConnectors { get; } = new Dictionary<IConnectable, IList<IConnectable>>();
        private IDictionary<IConnectable, FlowNode> RenderedNodes { get; } = new Dictionary<IConnectable, FlowNode>();
        private SearchWindowProvider SearchWindowProvider { get; set; }
        private EdgeConnectorListener EdgeConnectorListener { get; set; }

        public void Initialize()
        {
            UpdateViewTransform(FlowGraphParameters.instance.LatestPosition, FlowGraphParameters.instance.LatestScale);
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

            viewTransformChanged = graphView =>
            {
                FlowGraphParameters.instance.LatestPosition = graphView.viewTransform.position;
                FlowGraphParameters.instance.LatestScale = graphView.viewTransform.scale;
            };

            SearchWindowProvider = ScriptableObject.CreateInstance<SearchWindowProvider>();
            SearchWindowProvider.Initialize(this);
            EdgeConnectorListener = new EdgeConnectorListener(this, SearchWindowProvider);

            nodeCreationRequest = context =>
            {
                SearchWindowProvider.FlowPort = null;
                SearchWindow
                    .Open(
                        new SearchWindowContext(context.screenMousePosition),
                        SearchWindowProvider
                    );
            };

            var connectables = Selection.activeGameObject == default || Selection.activeGameObject.scene.IsValid()
                ? SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(x => x.GetComponentsInChildren<IConnectable>()).ToArray()
                : Selection.activeGameObject.GetComponentsInChildren<IConnectable>().ToArray();

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
                CreateNodeRecursive(rootConnectable, new Vector2(0, index * 400));
            }

            CreateEdges();
        }

        internal Vector2 NormalizeMousePosition(Vector2 mousePosition)
        {
            return contentViewContainer
                .WorldToLocal(
                    FlowEditorWindow
                        .Window
                        .rootVisualElement
                        .ChangeCoordinatesTo(
                            FlowEditorWindow.Window.rootVisualElement.parent,
                            mousePosition - FlowEditorWindow.Window.position.position
                        )
                );
        }

        private void CreateNodeRecursive(IConnectable connectable, Vector2 position)
        {
            var node = AddNode(connectable.GetType(), connectable, position);

            if (DestinationConnectors.ContainsKey(connectable))
            {
                foreach (var (targetConnectable, index) in DestinationConnectors[connectable].Select((v, i) => (v, i)))
                {
                    if (!RenderedNodes.ContainsKey(targetConnectable))
                    {
                        CreateNodeRecursive(targetConnectable, new Vector2(position.x + 350, position.y + index * 400));
                    }
                }
            }

            RenderedNodes[connectable] = node;
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

                    AddElement(AddEdge((FlowPort) RenderedNodes[connectable].OutputPort, (FlowPort) RenderedNodes[targetConnectable].InputPort));
                }
            }
        }

        public FlowNode AddNode(Type connectableType, IConnectable connectableInstance, Vector2 position)
        {
            var node = new FlowNode(ConnectableInfo.Create(connectableType, connectableInstance), EdgeConnectorListener);
            node.Initialize();
            node.SetPosition(new Rect(position.x, position.y, 0, 0));
            AddElement(node);
            return node;
        }

        public FlowEdge AddEdge(FlowPort outputPort, FlowPort inputPort)
        {
            var edge = new FlowEdge
            {
                output = outputPort,
                input = inputPort,
            };
            edge.output.Connect(edge);
            edge.input.Connect(edge);
            AddElement(edge);
            return edge;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports
                .ToList()
                .Where(x => x.direction != startPort.direction && x.node != startPort.node)
                .ToList();
        }
    }
}