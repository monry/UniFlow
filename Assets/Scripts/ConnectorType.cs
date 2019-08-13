namespace UniFlow
{
    public enum ConnectorType
    {
        LifecycleEvent = 1,
        UIBehaviourEventTrigger,
        TransformEvent,
        RectTransformEvent,
        CameraEvent,
        ParticleEvent,
        MouseEvent,
        KeyEvent,

        PhysicsCollisionEvent,
        PhysicsCollision2DEvent,
        PhysicsTriggerEvent,
        PhysicsTrigger2DEvent,

        AnimatorTrigger,
        SimpleAnimationController,
        AnimationEvent,
        AudioController,
        AudioEvent,
        PlayableController,
        TimelineSignal,

        LoadScene,
        UnloadScene,

        Timer,
        Interval,
        Empty,

        Custom,
    }
}