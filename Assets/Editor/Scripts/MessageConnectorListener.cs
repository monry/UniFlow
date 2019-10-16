using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace UniFlow.Editor
{
    public class MessageConnectorListener : IEdgeConnectorListener
    {
        public MessageConnectorListener(FlowGraphView flowGraphView)
        {
            FlowGraphView = flowGraphView;
        }

        private FlowGraphView FlowGraphView { get; }

        void IEdgeConnectorListener.OnDropOutsidePort(Edge edge, Vector2 position)
        {
            // Do nothing.
        }

        void IEdgeConnectorListener.OnDrop(GraphView graphView, Edge edge)
        {
            FlowEdge registeredEdge = default;
            switch (edge.output)
            {
                case FlowMessageComposePort _ when edge.input is FlowMessageCollectPort:
                    registeredEdge = FlowGraphView.AddEdge(edge.output, edge.input);
                    break;
                case FlowMessageCollectPort _ when edge.input is FlowMessageComposePort:
                    registeredEdge = FlowGraphView.AddEdge(edge.input, edge.output);
                    break;
            }

            if (registeredEdge == default)
            {
                return;
            }

            if (!(registeredEdge.output is FlowMessageComposePort messageComposePort) || !(registeredEdge.input is FlowMessageCollectPort messageCollectPort))
            {
                return;
            }

            messageCollectPort.CollectableMessageAnnotation.ValueCollector.SourceConnector = ((FlowNode) messageComposePort.node).ConnectorInfo.Connector;
            messageCollectPort.CollectableMessageAnnotation.ValueCollector.TargetConnector = ((FlowNode) messageCollectPort.node).ConnectorInfo.Connector;
            messageCollectPort.CollectableMessageAnnotation.ValueCollector.TypeString = messageComposePort.ComposableMessageAnnotation.Type.AssemblyQualifiedName;
            messageCollectPort.CollectableMessageAnnotation.ValueCollector.ComposerKey = messageComposePort.ComposableMessageAnnotation.Key;
            messageCollectPort.CollectableMessageAnnotation.ValueCollector.CollectorKey = messageCollectPort.CollectableMessageAnnotation.Key;
            EditorUtility.SetDirty(((FlowNode) messageCollectPort.node).ConnectorInfo.Connector as ConnectorBase);
        }
    }
}
