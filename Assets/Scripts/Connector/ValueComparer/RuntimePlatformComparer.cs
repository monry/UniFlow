using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    [AddComponentMenu("UniFlow/ValueComparer/RuntimePlatform", (int) ConnectorType.ValueComparerRuntimePlatform)]
    public class RuntimePlatformComparer : EnumComparerBase<RuntimePlatform, RuntimePlatformCollector>
    {
    }
}
