using UnityEngine;

namespace UniFlow.Connector.ValueComparer
{
    [AddComponentMenu("UniFlow/ValueComparer/RuntimePlatformComparer", (int) ConnectorType.ValueComparerRuntimePlatform)]
    public class RuntimePlatformComparer : EnumComparerBase<RuntimePlatform, RuntimePlatformCollector>
    {
    }
}
