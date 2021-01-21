using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace UniFlow.Editor
{
    public class EdgeConnectorListener : IEdgeConnectorListener
    {
        public EdgeConnectorListener(FlowGraphView flowGraphView, SearchWindowProvider searchWindowProvider)
        {
            FlowGraphView = flowGraphView;
            SearchWindowProvider = searchWindowProvider;
        }

        private FlowGraphView FlowGraphView { get; }
        private SearchWindowProvider SearchWindowProvider { get; }

        void IEdgeConnectorListener.OnDropOutsidePort(Edge edge, Vector2 position)
        {
            SearchWindowProvider.FlowPort = (FlowPort) (edge.output?.edgeConnector.edgeDragHelper.draggedPort ?? edge.input?.edgeConnector.edgeDragHelper.draggedPort);
            SearchWindow
                .Open(
                    new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                    SearchWindowProvider
                );
        }

        void IEdgeConnectorListener.OnDrop(GraphView graphView, Edge edge)
        {
            FlowGraphView.AddEdge((FlowPort) edge.output, (FlowPort) edge.input);
            (edge.output.node as FlowNode)?.ApplyTargetConnectors();
            FlowGraphView.SetupActAsTrigger();
        }
    }
}
