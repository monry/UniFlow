using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace UniFlow.Editor
{
    public class FlowPort : Port
    {
        private FlowPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
        }

        public static Port Create(Orientation portOrientation, Direction portDirection, Capacity portCapacity, IEdgeConnectorListener edgeConnectorListener = default)
        {
            var port = new FlowPort(portOrientation, portDirection, portCapacity, typeof(FlowPort));

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