using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
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

        public void Initialize()
        {
            var edge1 = new FlowEdge();
            var edge2 = new FlowEdge();

            {
                var flowNode = new FlowNode
                {
                    title = "hoge",
                };

                {
                    // Port の向きは Node から見た向き
                    var port = Port.Create<FlowEdge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(FlowPort));
                    port.portName = "Out";
                    port.portColor = Color.red;

                    // Edge も Node から見た向き
                    edge1.output = port;

                    // Add 対象の Container も Node から見た向き
                    flowNode.outputContainer.Add(port);
                }

                var properties = new VisualElement
                {
                    name = "MainContainer",
                };
                properties.Add(new Slider("Foo", 0.0f, 1.0f));
                flowNode.mainContainer.Add(properties);

                AddElement(flowNode);
            }
            {
                var flowNode = new FlowNode
                {
                    title = "fuga",
                };

                {
                    var port = Port.Create<FlowEdge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(FlowPort));
                    port.portName = "In";
                    port.portColor = Color.blue;

                    edge1.input = port;

                    flowNode.inputContainer.Add(port);
                }

                {
                    var port = Port.Create<FlowEdge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(FlowPort));
                    port.portName = "Out";
                    port.portColor = Color.red;

                    edge2.output = port;

                    flowNode.outputContainer.Add(port);
                }

                var properties = new VisualElement
                {
                    name = "MainContainer",
                };
                properties.Add(new Toggle("Bar"));
                properties
                    .Add(
                        new ObjectField("Quz")
                        {
                            objectType = typeof(StyleSheet),
                        }
                    );
                flowNode.mainContainer.Add(properties);

                AddElement(flowNode);
            }
            {
                var flowNode = new FlowNode
                {
                    title = "piyo",
                };

                {
                    var port = Port.Create<FlowEdge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(FlowPort));
                    port.portName = "In";
                    port.portColor = Color.blue;

                    edge2.input = port;

                    flowNode.inputContainer.Add(port);
                }

                var properties = new VisualElement
                {
                    name = "MainContainer",
                };
                properties.Add(new TextField("Buz"));
                flowNode.mainContainer.Add(properties);

                AddElement(flowNode);
            }

            AddElement(edge1);
            AddElement(edge2);
        }
    }
}