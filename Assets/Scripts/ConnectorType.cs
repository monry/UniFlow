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
        CreateInstance,
        DestroyInstance,
        SimpleAnimationController,
        SimpleAnimationEvent,
        AnimatorTrigger,
        AnimationEvent,
        AudioController,
        AudioEvent,
        PlayableController,
        TimelineSignal,
        RaycasterController,
        RaycastTargetController,
        MoveParentTransform,

        LoadScene                 = 400,
        LoadScene_Enum,
        UnloadScene,
        UnloadScene_Enum,

        Timer                     = 9000,
        Interval,
        TimeScaleController,
        Empty,

        Preset                    = 10000,

        Receiver                  = 20000,

        Custom                    = -1,
    }
}