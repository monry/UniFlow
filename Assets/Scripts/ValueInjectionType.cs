using JetBrains.Annotations;

namespace UniFlow
{
    [PublicAPI]
    public enum ValueInjectionType
    {
        Bool,
        Int,
        Float,
        String,
        Enum,
        Object,
        Vector2,
        Vector3,
        Vector4,
        Quaternion,
        Vector2Int,
        Vector3Int,
        Color,
        Color32,
    }
}
