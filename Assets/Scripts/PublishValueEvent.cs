using System;
using UnityEngine;
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

    [Serializable]
    public class PublishGameObjectEvent : UnityEvent<GameObject>
    {
    }

    [Serializable]
    public class PublishTransformEvent : UnityEvent<Transform>
    {
    }

    [Serializable]
    public class PublishRectTransformEvent : UnityEvent<RectTransform>
    {
    }

    [Serializable]
    public class PublishVector2Event : UnityEvent<Vector2>
    {
    }

    [Serializable]
    public class PublishVector3Event : UnityEvent<Vector3>
    {
    }

    [Serializable]
    public class PublishVector4Event : UnityEvent<Vector4>
    {
    }

    [Serializable]
    public class PublishQuaternionEvent : UnityEvent<Quaternion>
    {
    }

    [Serializable]
    public class PublishVector2IntEvent : UnityEvent<Vector2Int>
    {
    }

    [Serializable]
    public class PublishVector3IntEvent : UnityEvent<Vector3Int>
    {
    }

    [Serializable]
    public class PublishColorEvent : UnityEvent<Color>
    {
    }

    [Serializable]
    public class PublishColor32Event : UnityEvent<Color32>
    {
    }
}
