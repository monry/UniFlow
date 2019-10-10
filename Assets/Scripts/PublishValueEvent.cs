using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace UniFlow
{
    #region primitives

    [Serializable]
    public class PublishBoolEvent : UnityEvent<bool>
    {
    }

    [Serializable]
    public class PublishByteEvent : UnityEvent<byte>
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

    #endregion

    #region UnityEngine.Object

    [Serializable]
    public class PublishObjectEvent : UnityEvent<Object>
    {
    }

    [Serializable]
    public class PublishGameObjectEvent : UnityEvent<GameObject>
    {
    }

    [Serializable]
    public class PublishScriptableObjectEvent : UnityEvent<ScriptableObject>
    {
    }

    #endregion

    #region assets

    [Serializable]
    public class PublishTextureEvent : UnityEvent<Texture>
    {
    }

    [Serializable]
    public class PublishTexture2DEvent : UnityEvent<Texture2D>
    {
    }

    [Serializable]
    public class PublishTexture3DEvent : UnityEvent<Texture3D>
    {
    }

    [Serializable]
    public class PublishSpriteEvent : UnityEvent<Sprite>
    {
    }

    [Serializable]
    public class PublishAnimationClipEvent : UnityEvent<AnimationClip>
    {
    }

    [Serializable]
    public class PublishAudioClipEvent : UnityEvent<AudioClip>
    {
    }

    [Serializable]
    public class PublishTimelineAssetEvent : UnityEvent<TimelineAsset>
    {
    }

    #endregion

    #region components

    [Serializable]
    public class PublishComponentEvent : UnityEvent<Component>
    {
    }

    [Serializable]
    public class PublishMonoBehaviourEvent : UnityEvent<MonoBehaviour>
    {
    }

    [Serializable]
    public class PublishUIBehaviourEvent : UnityEvent<UIBehaviour>
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
    public class PublishAnimatorEvent : UnityEvent<Animator>
    {
    }

    [Serializable]
    public class PublishSimpleAnimationEvent : UnityEvent<SimpleAnimation>
    {
    }

    [Serializable]
    public class PublishAudioSourceEvent : UnityEvent<AudioSource>
    {
    }

    [Serializable]
    public class PublishPlayableDirectorEvent : UnityEvent<PlayableDirector>
    {
    }

    #endregion

    #region structs

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

    #endregion

    #region classes

    [Serializable]
    public class PublishAnimationEventEvent : UnityEvent<AnimationEvent>
    {
    }

    #endregion

    #region enums
    // If you can register a method that casts from int to enum, this region is unnecessary.

    [Serializable]
    public class PublishAnimatorCullingModeEvent : UnityEvent<AnimatorCullingMode>
    {
    }

    [Serializable]
    public class PublishAnimatorUpdateModeEvent : UnityEvent<AnimatorUpdateMode>
    {
    }

    [Serializable]
    public class PublishObservableTweenEaseTypeEvent : UnityEvent<ObservableTween.EaseType>
    {
    }

    [Serializable]
    public class PublishRuntimePlatformEvent : UnityEvent<RuntimePlatform>
    {
    }

    #endregion
}
