using UnityEditor.Experimental.GraphView;

namespace UniFlow.Editor
{
    public class FlowEdge : Edge, IRemovableElement
    {
        void IRemovableElement.RemoveFromGraphView()
        {
            output.Disconnect(this);
            input.Disconnect(this);

            if (!(output?.node is FlowNode outputNode))
            {
                return;
            }

            switch (output)
            {
                case FlowPort _:
                    outputNode.ApplyTargetConnectors();
                    break;
                case FlowValuePublishPort _:
                    outputNode.ApplyValuePublishers();
                    break;
            }
        }
    }
}
