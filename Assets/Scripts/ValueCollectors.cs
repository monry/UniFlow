using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UniFlow
{
#region primitives

    [Serializable]
    public class BoolCollector : ValueCollectorBase<bool>
    {
    }

    [Serializable]
    public class ByteCollector : ValueCollectorBase<byte>
    {
    }

    [Serializable]
    public class IntCollector : ValueCollectorBase<int>
    {
    }

    [Serializable]
    public class FloatCollector : ValueCollectorBase<float>
    {
    }

    [Serializable]
    public class StringCollector : ValueCollectorBase<string>
    {
    }

#endregion

#region UnityEngine.Object

    [Serializable]
    public class ObjectCollector : ValueCollectorBase<Object>
    {
    }

    [Serializable]
    public class GameObjectCollector : ValueCollectorBase<GameObject>
    {
    }

    [Serializable]
    public class ScriptableObjectCollector : ValueCollectorBase<ScriptableObject>
    {
    }

#endregion

#region assets

    [Serializable]
    public class TextureCollector : ValueCollectorBase<Texture>
    {
    }

    [Serializable]
    public class Texture2DCollector : ValueCollectorBase<Texture2D>
    {
    }

    [Serializable]
    public class Texture3DCollector : ValueCollectorBase<Texture3D>
    {
    }

    [Serializable]
    public class SpriteCollector : ValueCollectorBase<Sprite>
    {
    }

    [Serializable]
    public class AnimationClipCollector : ValueCollectorBase<AnimationClip>
    {
    }

    [Serializable]
    public class AudioClipCollector : ValueCollectorBase<AudioClip>
    {
    }

    [Serializable]
    public class TimelineAssetCollector : ValueCollectorBase<TimelineAsset>
    {
    }

#endregion

#region components

    [Serializable]
    public class ComponentCollector : ValueCollectorBase<Component>
    {
    }

    [Serializable]
    public class BehaviourCollector : ValueCollectorBase<Behaviour>
    {
    }

    [Serializable]
    public class MonoBehaviourCollector : ValueCollectorBase<MonoBehaviour>
    {
    }

    [Serializable]
    public class UIBehaviourCollector : ValueCollectorBase<UIBehaviour>
    {
    }

    [Serializable]
    public class TransformCollector : ValueCollectorBase<Transform>
    {
    }

    [Serializable]
    public class RectTransformCollector : ValueCollectorBase<RectTransform>
    {
    }

    [Serializable]
    public class AnimatorCollector : ValueCollectorBase<Animator>
    {
    }

    [Serializable]
    public class SimpleAnimationCollector : ValueCollectorBase<SimpleAnimation>
    {
    }

    [Serializable]
    public class AudioSourceCollector : ValueCollectorBase<AudioSource>
    {
    }

    [Serializable]
    public class PlayableDirectorCollector : ValueCollectorBase<PlayableDirector>
    {
    }

    [Serializable]
    public class ImageCollector : ValueCollectorBase<Image>
    {
    }

    [Serializable]
    public class RawImageCollector : ValueCollectorBase<RawImage>
    {
    }

#endregion

#region structs

    [Serializable]
    public class Vector2Collector : ValueCollectorBase<Vector2>
    {
    }

    [Serializable]
    public class Vector3Collector : ValueCollectorBase<Vector3>
    {
    }

    [Serializable]
    public class Vector4Collector : ValueCollectorBase<Vector4>
    {
    }

    [Serializable]
    public class QuaternionCollector : ValueCollectorBase<Quaternion>
    {
    }

    [Serializable]
    public class Vector2IntCollector : ValueCollectorBase<Vector2Int>
    {
    }

    [Serializable]
    public class Vector3IntCollector : ValueCollectorBase<Vector3Int>
    {
    }

    [Serializable]
    public class ColorCollector : ValueCollectorBase<Color>
    {
    }

    [Serializable]
    public class Color32Collector : ValueCollectorBase<Color32>
    {
    }

#endregion

#region classes

    [Serializable]
    public class AnimationEventCollector : ValueCollectorBase<AnimationEvent>
    {
    }

#endregion

#region enums

    // If you can register a method that casts from int to enum, this region is unnecessary.

    [Serializable]
    public class AnimatorCullingModeCollector : ValueCollectorBase<AnimatorCullingMode>
    {
    }

    [Serializable]
    public class AnimatorUpdateModeCollector : ValueCollectorBase<AnimatorUpdateMode>
    {
    }

    [Serializable]
    public class ObservableTweenEaseTypeCollector : ValueCollectorBase<ObservableTween.EaseType>
    {
    }

    [Serializable]
    public class RuntimePlatformCollector : ValueCollectorBase<RuntimePlatform>
    {
    }

    [Serializable]
    public class EaseTypeCollector : ValueCollectorBase<ObservableTween.EaseType>
    {
    }

#endregion
}
