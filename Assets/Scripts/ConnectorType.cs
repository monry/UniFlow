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
        InstantiateGameObject,
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
        KeyInputController,
        RaycasterController,
        RaycastTargetController,
        TransformSiblingController,
        MoveParentTransform,
        DontDestroyOnLoad,
        EventHandlingController,

        LoadScene                 = 400,
        LoadScene_Enum,
        LoadSceneEvent,
        UnloadScene,
        UnloadScene_Enum,
        UnloadSceneEvent,

        ValueProviderBool         = 1000,
        ValueProviderByte,
        ValueProviderInt,
        ValueProviderFloat,
        ValueProviderString,
        ValueProviderEnum,
        ValueProviderObject,
        ValueProviderGameObject,
        ValueProviderScriptableObject,
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
        ValueProviderCurrentRuntimePlatform,
        CustomValueProvider,

        ObjectListSelector        = 1500,
        GameObjectListSelector,
        ComponentListSelector,
        TransformListSelector,
        AudioClipListSelector,
        AnimationClipListSelector,
        TimelineAssetListSelector,
        CustomValueSelector,

        ValueComparerBool         = 2000,
        ValueComparerByte,
        ValueComparerInt,
        ValueComparerFloat,
        ValueComparerString,
        ValueComparerEnum,
        ValueComparerObject,
        ValueComparerGameObject,
        ValueComparerScriptableObject,
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
        ValueComparerRuntimePlatform,
        CustomValueComparer,

        ValueInjectorTransform    = 3000,
        ValueInjectorRectTransform,
        ValueInjectorImage,
        ValueInjectorRawImage,
        ValueInjectorTimelineControlTrack,
        ValueInjectorTimelineAudioTrack,
        ValueInjectorTimelineAnimationTrack,

        MusicPlayer               = 4000,
        MusicDuckingController,
        MusicPitchController,
        NumberImageRenderer,
        GaugeImageRenderer,
        ProgressiveTimer,

        SignalPublisher           = 6000,
        StringSignalPublisher,
        ScriptableObjectSignalPublisher,
        HandleEventSignalPublisher,
        MusicControlSignalPublisher,
        MusicDuckingSignalPublisher,
        MusicPitchSignalPublisher,

        SignalReceiver            = 6500,
        StringSignalReceiver,
        ScriptableObjectSignalReceiver,
        HandleEventSignalReceiver,
        MusicControlSignalReceiver,
        MusicDuckingSignalReceiver,
        MusicPitchSignalReceiver,

        And                       = 8000,
        Or,
        Xor,
        Not,

        BoolCalculator            = 8500,
        IntCalculator,
        FloatCalculator,
        StringCalculator,

        Timer                     = 9000,
        TimerFrame,
        Interval,
        IntervalFrame,
        TimeScaleController,
        RandomInt,
        RandomFloat,
        Counter,
        Empty,

        Toss                      = 9100,
        Receive                   = 9101,

        Preset                    = 10000,

        Receiver                  = 20000,

        Custom                    = -1,
    }
}
