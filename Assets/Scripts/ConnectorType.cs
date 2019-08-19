namespace UniFlow
{
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

        SimpleAnimationController = 300,
        AnimatorTrigger,
        AnimationEvent,
        AudioController,
        AudioEvent,
        PlayableController,
        TimelineSignal,
        RaycasterController,

        LoadScene                 = 400,
        UnloadScene,

        Timer                     = 9000,
        Interval,
        TimeScaleController,
        Empty,

        // ReSharper disable once UnusedMember.Global
        Custom                    = -1,
    }
}