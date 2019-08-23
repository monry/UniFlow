using System;
using UnityEditor.Experimental.GraphView;

namespace UniFlow.Editor
{
    public class FlowPort : Port
    {
        protected FlowPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
        }
    }
}