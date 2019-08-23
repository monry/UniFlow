using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace UniFlow.Editor
{
    public class FlowEdge : Edge
    {
        public FlowEdge()
        {

        }

        public override void OnSelected()
        {
            base.OnSelected();
            Debug.Log($"Input: {input.portName}");
            Debug.Log($"Output: {output.portName}");
        }
    }
}