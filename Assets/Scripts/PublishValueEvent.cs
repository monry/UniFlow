using System;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace UniFlow
{
    [Serializable]
    public class PublishBoolEvent : UnityEvent<bool>
    {
    }

    [Serializable]
    public class PublishIntEvent : UnityEvent<int>
    {
    }

    [Serializable]
    public class PublishFloatEvent : UnityEvent<float>
    {
    }

    [Serializable]
    public class PublishStringEvent : UnityEvent<string>
    {
    }

    [Serializable]
    public class PublishObjectEvent : UnityEvent<Object>
    {
    }
}
