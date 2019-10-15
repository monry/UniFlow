using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace UniFlow.Editor
{
    public class FlowMessageCollectPort : Port
    {
        private FlowMessageCollectPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
        }

        public CollectableMessageAnnotation CollectableMessageAnnotation { get; private set; }

        public static Port Create(Orientation portOrientation, Direction portDirection, Capacity portCapacity, CollectableMessageAnnotation collectableMessageAnnotation, IEdgeConnectorListener edgeConnectorListener = default)
        {
            var port = new FlowMessageCollectPort(portOrientation, portDirection, portCapacity, typeof(FlowMessageCollectPort))
            {
                CollectableMessageAnnotation = collectableMessageAnnotation,
            };

            // ReSharper disable once InvertIf
            if (edgeConnectorListener != default)
            {
                port.m_EdgeConnector = new EdgeConnector<FlowEdge>(edgeConnectorListener);
                port.AddManipulator(port.m_EdgeConnector);
            }

            return port;
        }
    }
}
