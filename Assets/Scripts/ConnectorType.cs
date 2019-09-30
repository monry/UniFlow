namespace UniFlow
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable UnusedMember.Global
    public enum ConnectorType
    {
        LifecycleEvent            = 100,
        UIBehaviourEventTrigger,
        TransformEvent,
        RectTransformEvent,
        CameraEvent,
        ParticleEvent,
        MouseEvent,
        KeyEvent,

        PhysicsCollisionEvent     = 200,
        PhysicsCollision2DEvent,
        PhysicsTriggerEvent,
        PhysicsTrigger2DEvent,

        ActivationController      = 300,
        InstantiateObject,
        DestroyInstance,
        SimpleAnimationController,
        SimpleAnimationEvent,
        AnimatorTrigger,
        AnimationEvent,
        AudioController,
        AudioEvent,
        PlayableController,
        TimelineEvent,
        TimelineSignal,
        RaycasterController,
        RaycastTargetController,
        MoveParentTransform,

        LoadScene                 = 400,
        LoadScene_Enum,
        UnloadScene,
        UnloadScene_Enum,

        ValueProvider             = 1000,
        ValueProviderBool,
        ValueProviderInt,
        ValueProviderFloat,
        ValueProviderString,
        ValueProviderEnum,
        ValueProviderObject,
        ValueProviderGameObject,
        ValueProviderTransform,
        ValueProviderRectTransform,
        ValueProviderVector2,
        ValueProviderVector3,
        ValueProviderVector4,
        ValueProviderQuaternion,
        ValueProviderVector2Int,
        ValueProviderVector3Int,
        ValueProviderColor,
        ValueProviderColor32,

        ValueComparerBool         = 2000,
        ValueComparerInt,
        ValueComparerFloat,
        ValueComparerString,
        ValueComparerEnum,
        ValueComparerObject,
        ValueComparerGameObject,
        ValueComparerTransform,
        ValueComparerRectTransform,
        ValueComparerVector2,
        ValueComparerVector3,
        ValueComparerVector4,
        ValueComparerQuaternion,
        ValueComparerVector2Int,
        ValueComparerVector3Int,
        ValueComparerColor,
        ValueComparerColor32,

        Timer                     = 9000,
        Interval,
        TimeScaleController,
        Empty,

        Toss                      = 9100,
        Receive                   = 9101,

        Preset                    = 10000,

        Receiver                  = 20000,

        Custom                    = -1,
    }
}
