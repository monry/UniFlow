using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using UniRx;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace UniFlow.Editor
{
    public class FlowNode : Node, IRemovableElement
    {
        public FlowNode(ConnectorInfo connectorInfo, IEdgeConnectorListener edgeConnectorListener, ValueInjectionConnectorListener valueInjectionConnectorListener)
        {
            ConnectorInfo = connectorInfo;
            EdgeConnectorListener = edgeConnectorListener;
            ValueInjectionConnectorListener = valueInjectionConnectorListener;
        }

        public void Initialize()
        {
            name = ConnectorInfo.Name;
            title = ConnectorInfo.Name;
            styleSheets.Add(AssetReferences.Instance.FlowNode);
            capabilities =
                Capabilities.Selectable |
//                Capabilities.Collapsible |
//                Capabilities.Resizable |
                Capabilities.Movable |
                Capabilities.Deletable |
                Capabilities.Droppable |
                Capabilities.Ascendable |
                Capabilities.Renamable;

            if (ConnectorInfo.GameObject == default)
            {
                ConnectorInfo.GameObject = DeterminateGameObject();
            }

            if (ConnectorInfo.Connector == default)
            {
                ConnectorInfo.Connector = Undo.AddComponent(ConnectorInfo.GameObject, ConnectorInfo.Type) as IConnector;
            }

            AddParameters();
            AddPorts();
            RegisterCallback((GeometryChangedEvent e) => ApplyPosition());
            ConnectorInfo
                .Connector?
                .OnConnectSubject
                .Subscribe(
                    x =>
                    {
                        var messagedBorder = new VisualElement {name = "messaged-border"};
                        Add(messagedBorder);
                        Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(_ => Remove(messagedBorder));
                    }
                );
        }

        public Port InputPort { get; private set; }
        public Port OutputPort { get; private set; }

        public IList<FlowValueReceivePort> ValueReceivePorts { get; } = new List<FlowValueReceivePort>();
        public IList<FlowValuePublishPort> ValuePublishPorts { get; } = new List<FlowValuePublishPort>();

        internal ConnectorInfo ConnectorInfo { get; }

        private IEdgeConnectorListener EdgeConnectorListener { get; }
        private IEdgeConnectorListener ValueInjectionConnectorListener { get; }

        private const string DefaultTargetGameObjectName = "UniFlowController";
        private static IEnumerable<string> ParameterNamesHideInNode { get; } = new List<string>
        {
            "actAsTrigger",
            "targetComponents",
            "targetIds",
        };

        private static IDictionary<Type, Func<ConnectorInfo, ConnectorInfo.Parameter, BindableElement>> CreateFieldFunctions { get; } = new Dictionary<Type, Func<ConnectorInfo, ConnectorInfo.Parameter, BindableElement>>
        {
            {typeof(string), CreateBindableElement<string, TextField>},
            {typeof(int), CreateBindableElement<int, IntegerField>},
            {typeof(float), CreateBindableElement<float, FloatField>},
            {typeof(double), CreateBindableElement<double, DoubleField>},
            {typeof(bool), CreateBindableElement<bool, Toggle>},
            {typeof(Object), CreateBindableElement<Object, ObjectField>},
        };

        private static IList<Type> PropertyFieldRenderableTypes { get; } = new List<Type>
        {
            typeof(byte),
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4),
            typeof(Quaternion),
            typeof(Vector2Int),
            typeof(Vector3Int),
            typeof(Color),
            typeof(Color32),
        };

        void IRemovableElement.RemoveFromGraphView()
        {
            if (ConnectorInfo.Connector == default || !(ConnectorInfo.Connector is Component component))
            {
                return;
            }

            Undo.DestroyObjectImmediate(component);
            FlowEditorWindow.Window.FlowGraphView.SetupActAsTrigger();
        }

        public Vector2 GetRecordedPosition()
        {
            return ((ConnectorBase) ConnectorInfo.Connector).FlowGraphNodePosition;
        }

        public void ApplyPosition()
        {
            if (!(ConnectorInfo.Connector is ConnectorBase connector) || connector == default || Mathf.Approximately(layout.position.magnitude, 0.0f))
            {
                return;
            }

            Undo.RecordObject(connector, "Move Node");
            // ReSharper disable once InvertIf
            if (connector.FlowGraphNodePosition != layout.position)
            {
                connector.FlowGraphNodePosition = layout.position;
                EditorUtility.SetDirty(connector);
            }
        }

        public void ApplyTargetConnectors()
        {
            if (ConnectorInfo.Connector == default || !(ConnectorInfo.Connector is ConnectorBase connector) || connector == default)
            {
                return;
            }

            Undo.RecordObject(connector, "Apply Target Connectors");
            connector.TargetComponents = OutputPort?
                .connections
                .Select(x => x.input.node as FlowNode)
                .Where(x => x != default)
                .Select(x => x.ConnectorInfo.Connector)
                .OfType<ConnectorBase>();
            EditorUtility.SetDirty(connector);
        }

        public void ApplyValuePublishers()
        {
            foreach (var valuePublishPort in ValuePublishPorts)
            {
                if (!(valuePublishPort.ValuePublisherInfo.PropertyInfo.GetValue(ConnectorInfo.Connector) is UnityEventBase unityEvent))
                {
                    continue;
                }
                // Preserve count before remove item from list
                var count = unityEvent.GetPersistentEventCount();
                for (var i = 0; i < count; i++)
                {
                    // Rebuild index every time, so remove first element
                    UnityEventTools.RemovePersistentListener(unityEvent, 0);
                }
                valuePublishPort
                    .connections
                    .Select(x => x.input as FlowValueReceivePort)
                    .Where(x => x?.node != default)
                    .ToList()
                    .ForEach(valuePublishPort.AddPersistentListener);
            }
        }

        private void AddParameters()
        {
            var contentsElement = this.Q("contents");

            var parametersElement = new VisualElement {name = "parameters"};
            {
                var dividerElement = new VisualElement {name = "divider"};
                dividerElement.AddToClassList("horizontal");
                parametersElement.Add(dividerElement);

                var itemsElement = new VisualElement {name = "items"};
                parametersElement.Add(itemsElement);

                {
                    var element = new VisualElement();
                    var field = new ObjectField("GameObject") {value = ConnectorInfo.GameObject, objectType = typeof(GameObject), allowSceneObjects = true};
                    field.RegisterValueChangedCallback(
                        x =>
                        {
                            var groupId = Undo.GetCurrentGroup();

                            if (x.previousValue != default && ConnectorInfo.Connector is Component component && component != default)
                            {
                                Undo.DestroyObjectImmediate(component);
                                EditorUtility.SetDirty(x.previousValue);
                            }

                            if (x.newValue != default && x.newValue is GameObject newGameObject)
                            {
                                ConnectorInfo.GameObject = newGameObject;
                                ConnectorInfo.Connector = Undo.AddComponent(ConnectorInfo.GameObject, ConnectorInfo.Type) as IConnector;
                                ConnectorInfo.ApplyParameters();
                                ApplyTargetConnectors();
                                InputPort?.connections.Select(y => y.output.node).OfType<FlowNode>().ToList().ForEach(y => y.ApplyTargetConnectors());
                                EditorUtility.SetDirty(x.newValue);
                            }

                            if (x.previousValue != default && x.previousValue is GameObject previousGameObject && previousGameObject.name == "Dummy")
                            {
                                Object.Destroy(previousGameObject);
                            }

                            Undo.CollapseUndoOperations(groupId);
                        }
                    );

                    element.Add(field);
                    itemsElement.Add(element);

                    if (ConnectorInfo.ParameterList.Any(x => !ParameterNamesHideInNode.Contains(x.Name)))
                    {
                        var innerDividerElement = new VisualElement {name = "divider"};
                        innerDividerElement.AddToClassList("horizontal");
                        itemsElement.Add(innerDividerElement);
                    }
                }

                foreach (var parameter in ConnectorInfo.ParameterList)
                {
                    if (ParameterNamesHideInNode.Contains(parameter.Name))
                    {
                        continue;
                    }

                    var element = new VisualElement();
                    var field = CreateField(ConnectorInfo, parameter);
                    element.Add(field);
                    itemsElement.Add(element);
                }
            }

            contentsElement.Add(parametersElement);
        }

        private static VisualElement CreateField(ConnectorInfo connectorInfo, ConnectorInfo.Parameter parameter)
        {
            // Generic types field only be rendered by PropertyField
            if (
                parameter.Type.IsGenericType
                && (
                    typeof(List<>).IsAssignableFrom(parameter.Type.GetGenericTypeDefinition())
                    || typeof(ExposedReference<>).IsAssignableFrom(parameter.Type.GetGenericTypeDefinition())
                )
                || PropertyFieldRenderableTypes.Contains(parameter.Type)
            )
            {
                if (connectorInfo.GameObject == default || connectorInfo.Connector == default)
                {
                    return null;
                }

                var serializedObject = new SerializedObject(connectorInfo.Connector as Component);
                var field = new PropertyField(serializedObject.FindProperty(parameter.Name), ToDisplayName(parameter.Name));
                field.Bind(serializedObject);
                return field;
            }

            // EnumField does not work unless the initial value is given to the second argument
            if (parameter.Type.IsEnum)
            {
                var field = new EnumField(ToDisplayName(parameter.Name), parameter.Value == default ? (Enum) Activator.CreateInstance(parameter.Type) : (Enum) parameter.Value);
                field.RegisterValueChangedCallback(
                    x =>
                    {
                        parameter.Value = x.newValue;
                        Undo.RecordObject(connectorInfo.Connector as Component, $"Change {connectorInfo.Name}.{parameter.Name}");
                        connectorInfo.ApplyParameter(parameter);
                        EditorUtility.SetDirty(connectorInfo.Connector as Component);
                    }
                );
                return field;
            }

            var fieldType = parameter.Type;

            if (typeof(Object).IsAssignableFrom(parameter.Type))
            {
                fieldType = typeof(Object);
            }

            if (!CreateFieldFunctions.ContainsKey(fieldType))
            {
                return null;
            }

            return CreateFieldFunctions[fieldType](connectorInfo, parameter);
        }

        private static BindableElement CreateBindableElement<TValue, TResult>(ConnectorInfo connectorInfo, ConnectorInfo.Parameter parameter) where TResult : BaseField<TValue>, new()
        {
            var field = new TResult
            {
                label = ToDisplayName(parameter.Name),
            };
            if (parameter.Value != default)
            {
                field.value = (TValue) parameter.Value;
            }

            if (field is INotifyValueChanged<TValue> notifyValueChanged)
            {
                notifyValueChanged.RegisterValueChangedCallback(
                    x =>
                    {
                        // 直接 Component 監視させちゃう？
                        Undo.RecordObject(FlowEditorWindow.Window, "Change Value");
                        parameter.Value = x.newValue;
                        connectorInfo.ApplyParameter(parameter);
                        FlowEditorWindow.Window.ForceRegisterUndo();
                        EditorUtility.SetDirty(connectorInfo.Connector as Component);
                    }
                );
            }

            // ReSharper disable once InvertIf
            if (typeof(TValue) == typeof(Object) && field is ObjectField objectField)
            {
                objectField.objectType = parameter.Type;
                objectField.allowSceneObjects = true;
            }

            return field;
        }

        private static GameObject DeterminateGameObject()
        {
            if (UniFlowSettings.instance.SelectedGameObject)
            {
                return UniFlowSettings.instance.SelectedGameObject;
            }

            if (GameObject.Find(DefaultTargetGameObjectName) != default)
            {
                return GameObject.Find(DefaultTargetGameObjectName);
            }

            var go = new GameObject(DefaultTargetGameObjectName);
            Undo.RegisterCreatedObjectUndo(go, "New UniFlowController");
            return go;
        }

        [SuppressMessage("ReSharper", "InvertIf")]
        private void AddPorts()
        {
            OutputPort = FlowPort.Create(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, EdgeConnectorListener);
            OutputPort.portName = "Out";
            outputContainer.Add(OutputPort);

            // Prevent grab port for input
            InputPort = FlowPort.Create(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            InputPort.portName = "In";
            inputContainer.Add(InputPort);

            if (ConnectorInfo.ValuePublishers.Any())
            {
                var divider = new VisualElement {name = "divider"};
                divider.AddToClassList("horizontal");
                outputContainer.Add(divider);

                foreach (var publisher in ConnectorInfo.ValuePublishers)
                {
                    var port = FlowValuePublishPort.Create(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, publisher, ValueInjectionConnectorListener);
                    port.portName = publisher.Name;
                    outputContainer.Add(port);
                    ValuePublishPorts.Add(port as FlowValuePublishPort);
                }
            }

            if (ConnectorInfo.ValueReceivers.Any())
            {
                var divider = new VisualElement {name = "divider"};
                divider.AddToClassList("horizontal");
                inputContainer.Add(divider);

                foreach (var receiver in ConnectorInfo.ValueReceivers)
                {
                    var port = FlowValueReceivePort.Create(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, receiver, ValueInjectionConnectorListener);
                    port.portName = receiver.Name;
                    inputContainer.Add(port);
                    ValueReceivePorts.Add(port as FlowValueReceivePort);
                }
            }
        }

        private static string ToDisplayName(string original)
        {
            if (Regex.IsMatch(original, "ControlMethod$"))
            {
                return "Control Method";
            }

            if (Regex.IsMatch(original, "EventType$"))
            {
                return "Event Type";
            }

            return original.Substring(0, 1).ToUpper() + Regex.Replace(original.Substring(1), "([A-Z]+[a-z])", " $1");
        }
    }
}
