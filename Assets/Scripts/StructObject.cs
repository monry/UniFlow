using System;
using UnityEngine;

namespace UniFlow
{
    [Serializable]
    public abstract class StructObject<T> : ScriptableObject where T : struct
    {
        [SerializeField] private T value = default;

        public T Value
        {
            get => value;
            set => this.value = value;
        }
    }

    [Serializable]
    [CreateAssetMenu(menuName = "UniFlow/StructObject/Vector2", fileName = "Vector2Object", order = (int) ValueInjectionType.Vector2)]
    public class Vector2Object : StructObject<Vector2>
    {
    }

    [Serializable]
    [CreateAssetMenu(menuName = "UniFlow/StructObject/Vector3", fileName = "Vector3Object", order = (int) ValueInjectionType.Vector3)]
    public class Vector3Object : StructObject<Vector3>
    {
    }

    [Serializable]
    [CreateAssetMenu(menuName = "UniFlow/StructObject/Vector4", fileName = "Vector4Object", order = (int) ValueInjectionType.Vector4)]
    public class Vector4Object : StructObject<Vector4>
    {
    }

    [Serializable]
    [CreateAssetMenu(menuName = "UniFlow/StructObject/Quaternion", fileName = "QuaternionObject", order = (int) ValueInjectionType.Quaternion)]
    public class QuaternionObject : StructObject<Quaternion>
    {
    }

    [Serializable]
    [CreateAssetMenu(menuName = "UniFlow/StructObject/Vector2Int", fileName = "Vector2IntObject", order = (int) ValueInjectionType.Vector2Int)]
    public class Vector2IntObject : StructObject<Vector2Int>
    {
    }

    [Serializable]
    [CreateAssetMenu(menuName = "UniFlow/StructObject/Vector3Int", fileName = "Vector3IntObject", order = (int) ValueInjectionType.Vector3Int)]
    public class Vector3IntObject : StructObject<Vector3Int>
    {
    }

    [Serializable]
    [CreateAssetMenu(menuName = "UniFlow/StructObject/Color", fileName = "ColorObject", order = (int) ValueInjectionType.Color)]
    public class ColorObject : StructObject<Color>
    {
    }

    [Serializable]
    [CreateAssetMenu(menuName = "UniFlow/StructObject/Color32", fileName = "Color32Object", order = (int) ValueInjectionType.Color32)]
    public class Color32Object : StructObject<Color32>
    {
    }
}
