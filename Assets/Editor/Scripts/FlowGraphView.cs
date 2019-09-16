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
        private static Vector2 NodesOffset { get; } = new Vector2(50.0f, 50.0f);
        private static Vector2 NodeMargin { get; } = new Vector2(50.0f, 50.0f);

        public void Initialize()
        {
            UpdateViewTransform(UniFlowSettings.instance.LatestPosition, UniFlowSettings.instance.LatestScale);
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

            SearchWindowProvider = ScriptableObject.CreateInstance<SearchWindowProvider>();
            SearchWindowProvider.Initialize(this);
            EdgeConnectorListener = new EdgeConnectorListener(this, SearchWindowProvider);

            viewTransformChanged += graphView =>
            {
                UniFlowSettings.instance.LatestPosition = graphView.viewTransform.position;
                UniFlowSettings.instance.LatestScale = graphView.viewTransform.scale;
            };

            nodeCreationRequest += context =>
            {
                SearchWindowProvider.FlowPort = null;
                SearchWindow
                    .Open(
                        new SearchWindowContext(context.screenMousePosition),
                        SearchWindowProvider
                    );
            };

            deleteSelection += (operationName, user) =>
            {
                DeleteSelection();
                SetupActAsTrigger();
            };

            elementsRemovedFromStackNode += (stackNode, removedGraphElements) =>
            {
                removedGraphElements.ToList().ForEach(Debug.Log);
                SetupActAsTrigger();
            };
            elementsRemovedFromGroup += (group, removedGraphElements) =>
            {
                removedGraphElements.ToList().ForEach(Debug.Log);
                SetupActAsTrigger();
            };

            graphViewChanged += change =>
            {
                change
                    .elementsToRemove?
                    .OfType<IRemovableElement>()
                    .ToList()
                    .ForEach(
                        x => x.RemoveFromGraphView()
                    );
                SetupActAsTrigger();
                return change;
            };

            CreateNodesFromInstance();

            CreateEdges();

            RegisterCallback(
                (GeometryChangedEvent e) =>
                {
                    Relocation();
                }
            );
            RegisterCallback(
                (KeyDownEvent e) =>
                {
                    // ReSharper disable once SwitchStatementMissingSomeCases
                    switch (e.keyCode)
                    {
                        case KeyCode.C:
                            SearchWindowProvider.FlowPort = null;
                            SearchWindow
                                .Open(
                                    new SearchWindowContext(GUIUtility.GUIToScreenPoint(layout.position) + new Vector2(125.0f, 55.0f)),
                                    SearchWindowProvider
                                );
                            break;
                    }
                }
            );
        }

        public void Relocation(bool forceReset = false)
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

                if (!forceReset && node.GetRecordedPosition().magnitude > 0.0f)
                {
                    node.SetPosition(new Rect(node.GetRecordedPosition(), node.layout.size));
                }
                else
                {
                    node.SetPosition(
                        new Rect(
                            new Vector2(normalizedPosition.x * (NodeWidth + NodeMargin.x) + NodesOffset.x, nextYList[normalizedPosition.x] + NodesOffset.y),
                            node.layout.size
                        )
                    );
                    node.ApplyPosition();
                }

                nextYList[normalizedPosition.x] += node.layout.height + NodeMargin.y;
            }

            Undo.CollapseUndoOperations(groupId);
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

        internal void SetupActAsTrigger()
        {
            this.Query()
                .Children<FlowNode>()
                .Where(x => x.ConnectableInfo.Connectable is ConnectorBase)
                .ForEach(x => ((ConnectorBase) x.ConnectableInfo.Connectable).ActAsTrigger = (x.InputPort.connections == null || !x.InputPort.connections.Any()));
        }

        private void CreateNodesFromInstance()
        {
            var connectables = UniFlowSettings.instance.IsPrefabMode
                ? UniFlowSettings.instance.SelectedGameObject
                    .GetComponentsInChildren<ConnectableBase>()
                    .ToArray()
                : (UniFlowSettings.instance.SelectedGameObject == default ? SceneManager.GetActiveScene() : UniFlowSettings.instance.SelectedGameObject.scene)
                    .GetRootGameObjects()
                    .SelectMany(x => x.GetComponentsInChildren<ConnectableBase>(true))
                    .ToArray();

            foreach (var connectable in connectables)
            {
                if (!DestinationConnectors.ContainsKey(connectable))
                {
                    DestinationConnectors[connectable] = new List<IConnectable>();
                }

                var connector = connectable as ConnectorBase;
                if (connector == default || connector.TargetComponents == null)
                {
                    continue;
                }

                foreach (var targetConnector in connector.TargetComponents)
                {
                    if (targetConnector == default)
                    {
                        continue;
                    }

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

            if (UniFlowSettings.instance.SelectedGameObject != default && !UniFlowSettings.instance.IsPrefabMode)
            {
                rootConnectables = rootConnectables.Where(x => x is ConnectorBase connector && connector != default && ContainsSelectedGameObjectInConnectableTree(connector)).ToArray();
            }

            foreach (var (rootConnectable, index) in rootConnectables.Select((v, i) => (v, i)))
            {
                CreateNodeRecursive(rootConnectable, new Vector2Int(0, index));
            }
        }

        private static bool ContainsSelectedGameObjectInConnectableTree(ConnectorBase haystack)
        {
            return haystack
                .TargetComponents
                .Any(
                    x =>
                        x.gameObject == UniFlowSettings.instance.SelectedGameObject
                        || x is ConnectorBase child && child != default && ContainsSelectedGameObjectInConnectableTree(child)
                );
        }

        private void CreateNodeRecursive(IConnectable connectable, Vector2Int normalizedPosition)
        {
            var node = AddNode(connectable.GetType(), connectable, Vector2.zero);
            NormalizedPositionDictionary[node] = normalizedPosition;
            RenderedNodes[connectable] = node;

            // ReSharper disable once InvertIf
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
            SetupActAsTrigger();
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
