using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace UniFlow.Editor
{
    public class FlowMessageComposePort : Port
    {
        private FlowMessageComposePort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
        }

        public ComposableMessageAnnotation ComposableMessageAnnotation { get; private set; }

        public static Port Create(Orientation portOrientation, Direction portDirection, Capacity portCapacity, ComposableMessageAnnotation composableMessageAnnotation, IEdgeConnectorListener edgeConnectorListener = default)
        {
            var port = new FlowMessageComposePort(portOrientation, portDirection, portCapacity, typeof(FlowMessageComposePort))
            {
                ComposableMessageAnnotation = composableMessageAnnotation,
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
