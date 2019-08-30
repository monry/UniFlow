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
        private IDictionary<FlowNode, Vector2Int> NormalizedPositionDictionary { get; } = new Dictionary<FlowNode, Vector2Int>();
        private SearchWindowProvider SearchWindowProvider { get; set; }
        private EdgeConnectorListener EdgeConnectorListener { get; set; }

        private const float NodeWidth = 300.0f;
        private static Vector2 NodeMargin { get; } = new Vector2(50.0f, 50.0f);

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

            CreateNodesFromInstance();

            CreateEdges();

            graphViewChanged += change =>
            {
                change.elementsToRemove?.ToList().ForEach(Debug.Log);
                change
                    .elementsToRemove?
                    .OfType<IRemovableElement>()
                    .ToList()
                    .ForEach(
                        x => x.RemoveFromGraphView()
                    );
                return change;
            };

            RegisterCallback(
                (GeometryChangedEvent e) =>
                {
                    Relocation();
                }
            );
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

        private void CreateNodesFromInstance()
        {
            var connectables = Selection.activeGameObject == default || Selection.activeGameObject.scene.IsValid()
                ? SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(x => x.GetComponentsInChildren<ConnectableBase>()).ToArray()
                : Selection.activeGameObject.GetComponentsInChildren<ConnectableBase>().ToArray();

            foreach (var connectable in connectables)
            {
                if (!DestinationConnectors.ContainsKey(connectable))
                {
                    DestinationConnectors[connectable] = new List<IConnectable>();
                }

                var connector = connectable as ConnectorBase;
                if (connector == default)
                {
                    continue;
                }

                foreach (var targetConnector in connector.TargetComponents)
                {
                    if (!SourceConnectors.ContainsKey(targetConnector))
                    {
                        SourceConnectors[targetConnector] = new List<IConnectable>();
                    }

                    DestinationConnectors[connectable].Add(targetConnector);
                    SourceConnectors[targetConnector].Add(connectable);
                }
            }

            var rootConnectables = connectables.Where(x => !SourceConnectors.ContainsKey(x)).ToArray();
            if (!rootConnectables.Any() && connectables.Any())
            {
                rootConnectables = new[] {connectables.First()};
            }

            foreach (var (rootConnectable, index) in rootConnectables.Select((v, i) => (v, i)))
            {
                CreateNodeRecursive(rootConnectable, new Vector2Int(0, index));
            }
        }

        private void CreateNodeRecursive(IConnectable connectable, Vector2Int normalizedPosition)
        {
            var node = AddNode(connectable.GetType(), connectable, Vector2.zero);
            NormalizedPositionDictionary[node] = normalizedPosition;
            RenderedNodes[connectable] = node;

            if (DestinationConnectors.ContainsKey(connectable))
            {
                foreach (var (targetConnectable, index) in DestinationConnectors[connectable].Select((v, i) => (v, i)))
                {
                    if (!RenderedNodes.ContainsKey(targetConnectable))
                    {
                        CreateNodeRecursive(targetConnectable, new Vector2Int(normalizedPosition.x + 1, normalizedPosition.y + index));
                    }
                    else if (NormalizedPositionDictionary[RenderedNodes[targetConnectable]].x < normalizedPosition.x + 1)
                    {
                        NormalizedPositionDictionary[RenderedNodes[targetConnectable]] = new Vector2Int(normalizedPosition.x + 1, normalizedPosition.y + index);
                    }
                }
            }
        }

        private void Relocation()
        {
            var nextYList = new List<float>();

            var groupId = Undo.GetCurrentGroup();

            foreach (var pair in NormalizedPositionDictionary)
            {
                var node = pair.Key;
                var normalizedPosition = pair.Value;
                if (nextYList.Count <= normalizedPosition.x)
                {
                    nextYList.Add(0.0f);
                }

                var position = node.GetRecordedPosition();
                Debug.Log(position);
                if (position.magnitude > 0.0f)
                {
                    node.SetPosition(new Rect(position, node.layout.size));
                }
                else
                {
                    node.SetPosition(
                        new Rect(
                            new Vector2(normalizedPosition.x * (NodeWidth + NodeMargin.x), nextYList[normalizedPosition.x]),
                            node.layout.size
                        )
                    );
                    node.ApplyPosition();
                }

                nextYList[normalizedPosition.x] += node.layout.height + NodeMargin.y;
            }

            Undo.CollapseUndoOperations(groupId);
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
            var connectableInfo = ConnectableInfo.Create(connectableType, connectableInstance);
            FlowEditorWindow.Window.ConnectableInfoList.Add(connectableInfo);
            var node = new FlowNode(connectableInfo, EdgeConnectorListener);
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