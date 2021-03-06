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
            }

            switch (input)
            {
                case FlowMessageCollectPort messageCollectPort:
                    messageCollectPort.CollectableMessageAnnotation.ValueCollector.SourceConnector = default;
                    messageCollectPort.CollectableMessageAnnotation.ValueCollector.TargetConnector = default;
                    messageCollectPort.CollectableMessageAnnotation.ValueCollector.TypeString = default;
                    messageCollectPort.CollectableMessageAnnotation.ValueCollector.CollectorKey = default;
                    messageCollectPort.CollectableMessageAnnotation.ValueCollector.ComposerKey = default;
                    break;
            }
        }
    }
}
