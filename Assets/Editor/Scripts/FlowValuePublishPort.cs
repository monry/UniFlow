using System;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Events;
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

        public void AddPersistentListener(FlowValueReceivePort valueReceivePort)
        {
            var publishConnectorInstance = (node as FlowNode)?.ConnectorInfo.Connector;
            var receivConnectorInstance = (valueReceivePort.node as FlowNode)?.ConnectorInfo.Connector;
            if (publishConnectorInstance == null || receivConnectorInstance == null)
            {
                return;
            }

            var setMethodInfo = valueReceivePort.ValueReceiverInfo.PropertyInfo.GetSetMethod(true);
            var unityEvent = ValuePublisherInfo.PropertyInfo.GetValue(publishConnectorInstance);

            // unityEvent.AddPersistentListener()
            unityEvent
                .GetType()
                .BaseType?
                .GetMethodsRecursive(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?
                .First(x => x.Name == "AddPersistentListener" && !x.GetParameters().Any())
                .Invoke(unityEvent, null);
            // unityEvent.m_PersistentCalls
            var persistentCalls = unityEvent
                .GetType()
                .GetFieldRecursive("m_PersistentCalls", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .GetValue(unityEvent);
            // unityEvent.m_PersistentCalls.RegisterObjectPersistentListener(unityEvent.GetPersistentEventCount() - 1, targetInstance, null, methodInfo.Name);
            persistentCalls
                .GetType()
                .GetMethodRecursive("RegisterObjectPersistentListener", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Invoke(
                    persistentCalls,
                    new object[]
                    {
                        (unityEvent as UnityEventBase)?.GetPersistentEventCount() - 1,
                        receivConnectorInstance,
                        null,
                        setMethodInfo.Name
                    }
                );
            // unityEvent.m_PersistentCalls.GetListener(unityEvent.GetPersistentEventCount() - 1)
            var persistentCall = persistentCalls
                .GetType()
                .GetMethodRecursive("GetListener", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Invoke(persistentCalls, new object[] {(unityEvent as UnityEventBase)?.GetPersistentEventCount() - 1});
            // unityEvent.m_PersistentCalls.GetListener(unityEvent.GetPersistentEventCount() - 1).m_Arguments
            var argumentCache = persistentCall
                .GetType()
                .GetFieldRecursive("m_Arguments", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .GetValue(persistentCall);
            // unityEvent.m_PersistentCalls.GetListener(unityEvent.GetPersistentEventCount() - 1).m_Arguments.m_ObjectArgumentAssemblyTypeName = setMethodInfo.GetParameters().First().ParameterType.AssemblyQualifiedName
            argumentCache
                .GetType()
                .GetFieldRecursive("m_ObjectArgumentAssemblyTypeName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .SetValue(argumentCache, setMethodInfo.GetParameters().First().ParameterType.AssemblyQualifiedName);
            // unityEvent.m_PersistentCalls.GetListener(unityEvent.GetPersistentEventCount() - 1).m_Mode = PersistentListenerMode.EventDefined
            persistentCall
                .GetType()
                .GetFieldRecursive("m_Mode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .SetValue(persistentCall, PersistentListenerMode.EventDefined);
        }
    }
}
