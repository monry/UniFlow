using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Events;
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

        private static IDictionary<Type, Action<object, object>> AddPersistentListenerCallbackMap { get; } = new Dictionary<Type, Action<object, object>>
        {
            {typeof(bool), (unityEvent, unityAction) => UnityEventTools.AddBoolPersistentListener(unityEvent as UnityEvent<bool>, unityAction as UnityAction<bool>, default)},
            {typeof(int), (unityEvent, unityAction) => UnityEventTools.AddIntPersistentListener(unityEvent as UnityEvent<int>, unityAction as UnityAction<int>, default)},
            {typeof(float), (unityEvent, unityAction) => UnityEventTools.AddFloatPersistentListener(unityEvent as UnityEvent<float>, unityAction as UnityAction<float>, default)},
            {typeof(string), (unityEvent, unityAction) => UnityEventTools.AddStringPersistentListener(unityEvent as UnityEvent<string>, unityAction as UnityAction<string>, default)},
        };

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
            var unityAction = Delegate
                .CreateDelegate(
                    typeof(UnityAction<>).MakeGenericType(setMethodInfo.GetParameters().First().ParameterType),
                    receivConnectorInstance,
                    setMethodInfo.Name,
                    false
                );

            if (AddPersistentListenerCallbackMap.ContainsKey(ValuePublisherInfo.Type))
            {
                AddPersistentListenerCallbackMap[ValuePublisherInfo.Type].Invoke(unityEvent, unityAction);
            }
            else
            {
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
            }
        }
    }
}
