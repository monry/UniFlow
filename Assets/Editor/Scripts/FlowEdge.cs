using UnityEditor.Experimental.GraphView;

namespace UniFlow.Editor
{
    public class FlowEdge : Edge, IRemovableElement
    {
        void IRemovableElement.RemoveFromGraphView()
        {
            output.Disconnect(this);
            (output?.node as FlowNode)?.ApplyTargetConnectors();
        }
    }
}