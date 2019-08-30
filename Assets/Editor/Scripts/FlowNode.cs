using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace UniFlow.Editor
{
    public class FlowNode : Node, IRemovableElement
    {
        public FlowNode(ConnectableInfo connectableInfo, IEdgeConnectorListener edgeConnectorListener)
        {
            ConnectableInfo = connectableInfo;
            EdgeConnectorListener = edgeConnectorListener;
        }

        public void Initialize()
        {
            title = ConnectableInfo.Name;
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

            AddParameters();
            AddPorts();
            RegisterCallback((GeometryChangedEvent e) => ApplyPosition());
        }

        public Port InputPort { get; private set; }
        public Port OutputPort { get; private set; }

        private ConnectableInfo ConnectableInfo { get; }
        private IEdgeConnectorListener EdgeConnectorListener { get; }

        private static IDictionary<Type, Func<ConnectableInfo.Parameter, BindableElement>> CreateFieldFunctions { get; } = new Dictionary<Type, Func<ConnectableInfo.Parameter, BindableElement>>
        {
            {typeof(string), CreateBindableElement<string, TextField>},
            {typeof(int), CreateBindableElement<int, IntegerField>},
            {typeof(float), CreateBindableElement<float, FloatField>},
            {typeof(double), CreateBindableElement<double, DoubleField>},
            {typeof(bool), CreateBindableElement<bool, Toggle>},
            {typeof(Object), CreateBindableElement<Object, ObjectField>},
        };

        void IRemovableElement.RemoveFromGraphView()
        {
            if (ConnectableInfo.Connectable == default || !(ConnectableInfo.Connectable is Component component))
            {
                return;
            }

            Undo.DestroyObjectImmediate(component);
        }

        public Vector2 GetRecordedPosition()
        {
            return ((ConnectableBase) ConnectableInfo.Connectable).FlowGraphNodePosition;
        }

        public void ApplyPosition()
        {
            if (!(ConnectableInfo.Connectable is ConnectableBase connectable) || Mathf.Approximately(layout.position.magnitude, 0.0f))
            {
                return;
            }

            Undo.RecordObject(connectable, "Move Node");
            connectable.FlowGraphNodePosition = layout.position;
            EditorUtility.SetDirty(connectable);
        }

        public void ApplyTargetConnectors()
        {
            if (ConnectableInfo.Connectable == default || !(ConnectableInfo.Connectable is ConnectorBase connector))
            {
                return;
            }

            Undo.RecordObject(connector, "Apply Target Connectors");
            connector.TargetComponents = OutputPort?
                .connections
                .Select(x => x.input.node as FlowNode)
                .Where(x => x != default)
                .Select(x => x.ConnectableInfo.Connectable)
                .OfType<ConnectableBase>();
            EditorUtility.SetDirty(connector);
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
                    var field = new ObjectField("GameObject") {value = ConnectableInfo.GameObject, objectType = typeof(GameObject), allowSceneObjects = true};
                    field.RegisterValueChangedCallback(
                        x =>
                        {
                            var groupId = Undo.GetCurrentGroup();

                            if (x.previousValue != default && ConnectableInfo.Connectable != default)
                            {
                                Undo.DestroyObjectImmediate(ConnectableInfo.Connectable as Component);
                                EditorUtility.SetDirty(x.previousValue);
                            }

                            if (x.newValue != default && x.newValue is GameObject newGameObject)
                            {
                                ConnectableInfo.GameObject = newGameObject;
                                ConnectableInfo.Connectable = Undo.AddComponent(ConnectableInfo.GameObject, ConnectableInfo.Type) as IConnectable;
                                ConnectableInfo.ApplyParameters();
                                ApplyTargetConnectors();
                                InputPort?.connections.Select(y => y.output.node).OfType<FlowNode>().ToList().ForEach(y => y.ApplyTargetConnectors());
                                EditorUtility.SetDirty(x.newValue);
                            }

                            Undo.CollapseUndoOperations(groupId);
                        }
                    );

                    element.Add(field);
                    itemsElement.Add(element);

                    var innerDividerElement = new VisualElement {name = "divider"};
                    innerDividerElement.AddToClassList("horizontal");
                    itemsElement.Add(innerDividerElement);
                }

                foreach (var parameter in ConnectableInfo.ParameterList)
                {
                    var element = new VisualElement();
                    var field = CreateField(ConnectableInfo, parameter);
                    element.Add(field);
                    itemsElement.Add(element);
                }
            }

            contentsElement.Add(parametersElement);
        }

        private static BindableElement CreateField(ConnectableInfo connectableInfo, ConnectableInfo.Parameter parameter)
        {
            // EnumField does not work unless the initial value is given to the second argument
            if (parameter.Type.IsEnum)
            {
                var field = new EnumField(ToDisplayName(parameter.Name), parameter.Value == default ? (Enum) Activator.CreateInstance(parameter.Type) : (Enum) parameter.Value);
                field.RegisterValueChangedCallback(
                    x =>
                    {
                        parameter.Value = x.newValue;
                        Undo.RecordObject(connectableInfo.Connectable as Component, $"Change {connectableInfo.Name}.{parameter.Name}");
                        connectableInfo.ApplyParameter(parameter);
                        EditorUtility.SetDirty(connectableInfo.Connectable as Component);
                    }
                );
                return field;
            }

            var fieldType = parameter.Type;

            if (typeof(Object).IsAssignableFrom(parameter.Type))
            {
                fieldType = typeof(Object);
            }

            return CreateFieldFunctions[fieldType](parameter);
        }

        private static BindableElement CreateBindableElement<TValue, TResult>(ConnectableInfo.Parameter parameter) where TResult : BaseField<TValue>, new()
        {
            var field = new TResult
            {
                label = ToDisplayName(parameter.Name),
                value = (TValue) parameter.Value,
            };

            if (field is INotifyValueChanged<TValue> notifyValueChanged)
            {
                notifyValueChanged.RegisterValueChangedCallback(
                    x =>
                    {
                        // 直接 Component 監視させちゃう？
                        Undo.RecordObject(FlowEditorWindow.Window, "Change Value");
                        parameter.Value = x.newValue;
                        FlowEditorWindow.Window.ForceRegisterUndo();
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

        private void AddPorts()
        {
            if (typeof(IConnector).IsAssignableFrom(ConnectableInfo.Type))
            {
                OutputPort = FlowPort.Create(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, EdgeConnectorListener);
                OutputPort.portName = "Out";
                outputContainer.Add(OutputPort);
            }
            else
            {
                outputContainer.RemoveFromHierarchy();
            }

            InputPort = FlowPort.Create(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            InputPort.portName = "In";
            inputContainer.Add(InputPort);
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