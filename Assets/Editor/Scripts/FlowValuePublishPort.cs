using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace UniFlow.Editor
{
    public class FlowValuePublishPort : Port
    {
        private FlowValuePublishPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
        }

        public ConnectorInfo.ValuePublisherInfo ValuePublisherInfo { get; private set; }

        public static Port Create(Orientation portOrientation, Direction portDirection, Capacity portCapacity, ConnectorInfo.ValuePublisherInfo valuePublisherInfo, IEdgeConnectorListener edgeConnectorListener = default)
        {
            var port = new FlowValuePublishPort(portOrientation, portDirection, portCapacity, typeof(FlowValuePublishPort))
            {
                ValuePublisherInfo = valuePublisherInfo,
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
