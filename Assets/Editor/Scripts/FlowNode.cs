using UnityEditor.Experimental.GraphView;

namespace UniFlow.Editor
{
    public class FlowNode : Node
    {
        public FlowNode()
        {
            styleSheets.Add(AssetReferences.Instance.FlowNode);
            capabilities =
                Capabilities.Selectable |
//                Capabilities.Collapsible |
                Capabilities.Resizable |
                Capabilities.Movable |
                Capabilities.Deletable |
                Capabilities.Droppable |
                Capabilities.Ascendable |
                Capabilities.Renamable;
        }
    }
}