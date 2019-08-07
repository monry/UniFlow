# Event Connector

EventConnector is a library that can connect various Unity events including user interaction without writing any C# script.

Processes such as "Tutorial that accepts user interaction" and "Waiting for the end of playback of Animation, Audio, Timeline, etc." can be implemented easily.

## Installation

```bash
upm add package dev.monry.upm.eventconnector
```

Please refer to [this repository](https://github.com/upm-packages/upm-cli) about the `upm` command.

## Usages

### Basics

#### 1. Attach EventPublishers

Attach one or more Components what inherits `EventPublisher` listed below into GameObject.

#### 2. Implement EventReceiver

Implement Component what inherits `EventConnector.EventReceiver`.

Implement the process you want to execute when the event is received in the `OnReceive()` method that needs to override by `EventReceiver`.

This method will be passed `EventMessages` what contrains all propagated event informations.

#### 3. Connect EventConnectors and EventReceiver

Set **Previous EventConnector** into *Source Connector Instances* field for each EventConnectors and EventReceiver.

It is also possible to solve with `Zenject.ResolveIdAll<T>()` by setting ID in *Source Connector Ids* field

### Inspector

<img width="317" alt="inspector" src="https://user-images.githubusercontent.com/838945/62629227-bf136480-b967-11e9-850e-e336c1e912db.png">

#### Target Connector Instances

Specify instances what inhreits `EventPublisher` or `EventReceiver` into this field.

Fire messages at the correct time for each component.

#### Target Connector Instances

Specify IDs what provides instances of `EventPublisher` or `EventReceiver` resolved by `Zenject.ResolveIdAll()` into this field.

Fire messages at the correct time for each component.

#### Act As Trigger

Set true to allow to act as the entry point of events.

#### Act As Receiver

Set true to allow to act as the receiver of events.

#### Other parameters for each Publisher

Individual parameters can be specified for each component.

## Components what inherits `EventPublisher`

### Messaging from traditional callback

#### `LifecycleEvent`

This component observes lifecycle events like as `Start`, `Update`, `FixedUpdate`, `OnEnable`, `Destroy`, ...

If you do not specify instance of `Component` will be used self instance.

Component supports below events to observe.

* Start
* Update
* FixedUpdate
* LateUpdate
* OnEnable
* OnDisable
* OnDestroy

#### `UIBehaviourEventTrigger`

This component observes events like as `OnPointerXxx`, `OnDrag`, ...

If you do not specify instance of `UIBehaviour` will be obtained by `GetComponent<UIBehaviour>()`

Component supports below events to observe.

* PointerEnter
* PointerExit
* PointerDown
* PointerUp
* PointerClick
* Drag
* Drop
* Scroll
* UpdateSelected
* Select
* Deselect
* Move
* InitializePotentialDrag
* BeginDrag
* EndDrag
* Submit
* Cancel

#### `TransformEvent`

This component observes Transform events.

If you do not specify instance of `Component` will be obtained by `this.transform`.

Component supports below events to observe.

* BeforeTransformParentChanged
* TransformParentChanged
* TransformChildrenChanged

#### `RectTransformEvent`

This component observes RectTransform events.

If you do not specify instance of `Component` will be obtained by `GetComponent<RectTransform>()`

Component supports below events to observe.

* CanvasGroupChanged
* RectTransformDimensionsChange
* RectTransformRemoved

#### `CameraEvent`

This component observes Camera events.

If you do not specify instance of `Component` will be used self instance.

Component supports below events to observe.

* BecomeVisible
* BecomeInvisible

#### `ParticleEvent`

This component observes Particle events.

If you do not specify instance of `Component` will be used self instance.

Component supports below events to observe.

* ParticleCollision
* ParticleTrigger

#### `MouseEvent`

This component observes Mouse events.

If you do not specify instance of `Component` will be used self instance.

Component supports below events to observe.

* MouseDown
* MouseUp
* MouseUpAsButton
* MouseEnter
* MouseExit
* MouseOver
* MouseDrag

### Messaging from Physics

#### `PhysicsCollisionEvent`

This component observes PhysicsCollision events.

If you do not specify instance of `Component` will be used self instance.

Component supports below events to observe.

* CollisionEnter
* CollisionExit
* CollisionStay

#### `PhysicsCollision2DEvent`

This component observes PhysicsCollision2D events.

If you do not specify instance of `Component` will be used self instance.

Component supports below events to observe.

* CollisionEnter2D
* CollisionExit2D
* CollisionStay2D

#### `PhysicsTriggerEvent`

This component observes PhysicsTrigger events.

If you do not specify instance of `Component` will be used self instance.

Component supports below events to observe.

* TriggerEnter
* TriggerExit
* TriggerStay

#### `PhysicsTrigger2DEvent`

This component observes PhysicsTrigger2D events.

If you do not specify instance of `Component` will be used self instance.

Component supports below events to observe.

* TriggerEnter2D
* TriggerExit2D
* TriggerStay2D

### Messaging from time related components

#### `AnimatorTrigger`

This component will fire `Animator.SetTrigger()`.

If you do not specify instance of `Animator` will be obtained by `GetComponent<Animator>()`

Note: Currently, parameter invocation such as `SetInt()` is not supported.

#### `AnimationEvent`

This component observes AnimationEvent fireing.

To receive AnimationEvent, Component needs to be attached to the same GameObject as Animator.

#### `AudioController`

This component will fire `AudioSource.Xxx()`.

If you do not specify instance of `AudioSource` will be obtained by `GetComponent<AudioSource>()`

Suppoeted methods are listed below.

* Play
* Stop
* Pause
* UnPause

#### `AudioEvent`

This component observes state changes of AudioSource.

If you do not specify instance of `AudioSource` will be obtained by `GetComponent<AudioSource>()`

Suppoeted events are listed below.

* Play
* Stop
* Pause
* UnPause
* Loop

#### `PlayableController`

This component will fire `PlayableDirector.Play()`.

If you do not specify instance of `PlayableDirector` will be obtained by `GetComponent<PlayableDirector>()`

#### `TimelineSignal`

This component observes Timeline Signals emission.

Specify `TimelineSignal.Dispatch()` in the destination method of the `UnityEngine.Timeline.SignalReceiver` component

### Other utilities

#### `Timer`

This component observes time specified by inspector.

#### `Interval`

This component observes interval specified by inspector.

#### `Empty`

This component nothing to observing and firing.

## License

* [OtoLogic (https://otologic.jp)](https://otologic.jp) (CC BY 4.0)
    * `Assets/Tests/Runtime/Sounds/TimeReport.mp3`
