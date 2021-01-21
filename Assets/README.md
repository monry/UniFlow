# UniFlow

UniFlow is a library that can connect various Unity components without writing any C# script.

You can implement easily processes such as "Tutorial that accepts user interaction" and "Waiting for the end of playback of Animation, Audio, Timeline, etc.".

## Installation

```bash
upm add package dev.monry.uniflow
```

Please refer to [this repository](https://github.com/upm-packages/upm-cli) about the `upm` command.

## Usages

### Basics

#### 1. Attach Connectors

Attach one or more Components what implements `ConnectorBase` listed below into GameObject.

#### 2. Connect Connectors

Set *Next `ConnectorBase`* into **Target Instances** field for each Connectors.

It is also possible to solve with `Zenject.ResolveIdAll<T>()` by setting ID in **Target Ids** field

#### 3. Implement Receiver

Implement Component what inherits `ReceiverBase`.

Implement the process you want to execute when the event is received in the `OnReceive()` method that needs to override by `ReceiverBase`.

This method will be passed `EventMessages` what contains all propagated event informations.

### Inspector

<img width="317" alt="inspector" src="https://user-images.githubusercontent.com/838945/63312381-5dc29c80-c33c-11e9-9dd7-e29c4e2b068d.png">

#### Target Instances

Specify instances what inherits `ConnectableBase` into this field.<br />
\* `ConnectorBase` and `ReceiverBase` are inherits `ConnectableBase`

Fire messages at the correct time for each component.

#### Target Ids

Specify IDs what provides instances of `IConnectable` resolved by `Zenject.ResolveIdAll()` into this field.

Fire messages at the correct time for each component.

#### Act As Trigger

Set true to allow to act as the entry point of events.

#### Other parameters for each Connector

Individual parameters can be specified for each component.

### UniFlow Graph

![image](https://user-images.githubusercontent.com/838945/64085909-463dd780-cd70-11e9-9192-77972dbcebce.png)

UniFlow provides GraphView to view/edit connectables in scenes/prefabs

## Components what inherits `ConnectorBase`

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

Note: Do not notify on mobile platforms.

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

#### `ActivationController`

This component will control activation of `GameObject` and `MonoBehaviour`.

Invoke `GameObject.SetActive(bool)` if `TargetGameObjects` field specified.

Change `MonoBehaviour.enabled` field if `TargetMonoBaheviours` field specified.

#### `DestroyInstance`

This component will destroy instances.

#### `SimpleAnimationController`

This component will control [SimpleAnimation](https://github.com/Unity-Technologies/SimpleAnimation) components.

#### `SimpleAnimationEvent`

This component will observe event that triggered from [SimpleAnimation](https://github.com/Unity-Technologies/SimpleAnimation) components.

#### `AnimatorTrigger`

This component will fire `Animator.SetTrigger()`.

If you do not specify instance of `Animator` will be obtained by `GetComponent<Animator>()`

Note: Currently, parameter invocation such as `SetInt()` is not supported.

#### `AnimationEvent`

This component observes AnimationEvent firing.

To receive AnimationEvent, Component needs to be attached to the same GameObject as Animator.

#### `AudioController`

This component will fire `AudioSource.Xxx()`.

If you do not specify instance of `AudioSource` will be obtained by `GetComponent<AudioSource>()`

Supported methods are listed below.

* Play
* Stop
* Pause
* UnPause

#### `AudioEvent`

This component observes state changes of AudioSource.

If you do not specify instance of `AudioSource` will be obtained by `GetComponent<AudioSource>()`

Supported events are listed below.

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

#### `RaycasterController`

This component controls some Raycaster such as `PhysicsRaycaster`, `Physics2DRaycaster` and `GraphicRaycaster`.

#### `RaycastTargetController`

This component controls raycastTarget such as `Collider.enabled`, `Graphic.raycastTarget`.

#### `MoveParentTransform`

This component changes parent transform.

### Scene management

#### `LoadScene`

This component will load scene specified as string.

#### `LoadScene<TEnum>`

This component will load scene specified as enum.

#### `UnloadScene`

This component will unload scene specified as string.

#### `UnloadScene<TEnum>`

This component will unload scene specified as enum.

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
