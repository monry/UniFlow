using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace UniFlow.Editor
{
    public class ValueInjectionConnectorListener : IEdgeConnectorListener
    {
        public ValueInjectionConnectorListener(FlowGraphView flowGraphView)
        {
            FlowGraphView = flowGraphView;
        }

        private FlowGraphView FlowGraphView { get; }

        void IEdgeConnectorListener.OnDropOutsidePort(Edge edge, Vector2 position)
        {
            // Do nothing
        }

        void IEdgeConnectorListener.OnDrop(GraphView graphView, Edge edge)
        {
            FlowEdge registeredEdge = default;
            switch (edge.output)
            {
                case FlowValuePublishPort _ when edge.input is FlowValueReceivePort:
                    registeredEdge = FlowGraphView.AddEdge(edge.output, edge.input);
                    break;
                case FlowValueReceivePort _ when edge.input is FlowValuePublishPort:
                    registeredEdge = FlowGraphView.AddEdge(edge.input, edge.output);
                    break;
            }

            if (registeredEdge == default)
            {
                return;
            }

            if (!(registeredEdge.output is FlowValuePublishPort publishPort) || !(registeredEdge.input is FlowValueReceivePort receivePort))
            {
                return;
            }

            publishPort.AddPersistentListener(receivePort);
        }
    }
}
