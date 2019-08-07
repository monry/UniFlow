namespace EventConnector
{
    public enum EventType
    {
        LifecycleEvent = 1,
        UIBehaviourEventTrigger,
        TransformEvent,
        RectTransformEvent,
        CameraEvent,
        ParticleEvent,
        MouseEvent,

        PhysicsCollisionEvent,
        PhysicsCollision2DEvent,
        PhysicsTriggerEvent,
        PhysicsTrigger2DEvent,

        AnimatorTrigger,
        AnimationEvent,
        AudioController,
        AudioEvent,
        PlayableController,
        TimelineSignal,

        Timer,
        Interval,
        Empty,
    }
}