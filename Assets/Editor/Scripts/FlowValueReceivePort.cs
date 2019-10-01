using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace UniFlow.Editor
{
    public class FlowValueReceivePort : Port
    {
        private FlowValueReceivePort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
        }

        public ConnectorInfo.ValueReceiverInfo ValueReceiverInfo { get; private set; }

        public static Port Create(Orientation portOrientation, Direction portDirection, Capacity portCapacity, ConnectorInfo.ValueReceiverInfo valueReceiverInfo, IEdgeConnectorListener edgeConnectorListener = default)
        {
            var port = new FlowValueReceivePort(portOrientation, portDirection, portCapacity, typeof(FlowValueReceivePort))
            {
                ValueReceiverInfo = valueReceiverInfo,
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
