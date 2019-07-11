# Event Connector

Connect arbitrary events that fired in different GameObjects, Components.

## Installation

```bash
upm add package dev.monry.upm.eventconnector
```

## Usages

![EventConnector](https://user-images.githubusercontent.com/838945/61065477-b732d400-a43e-11e9-8392-150bddb666db.gif)

### 1. Attach EventConnectors

Attach one or more Components what inherits `EventConnector.EventConnector` listed below into GameObject.

### 2. Implement EventReceiver

Implement Component what inherits `EventConnector.EventReceiver`.

Implement the process you want to execute when the event is received in the `Receive()` method that needs to override by `EventReceiver`.

This method will be passed `EventMessages` what contrains all propagated event informations.

### 3. Connect EventConnectors and EventReceiver

Set **Previous EventConnector** into *Source Connector Instances* field for each EventConnectors and EventReceiver.

It is also possible to solve with `Zenject.ResolveIdAll<T>()` by setting ID in *Source Connector Ids* field

## Components

### `UIBehaviourEventTrigger`

This component observes events like as `OnPointerXxx`, `OnDrag`, ...

If you do not specify instance of `UIBehaviour` will be obtained by `GameObject.GetComponent<UIBehaviour>()`

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

### `AnimatorTrigger`

This component will fire `Animator.SetTrigger()`.

If you do not specify instance of `Animator` will be obtained by `GameObject.GetComponent<Animator>()`

Note: Currently, parameter invocation such as `SetInt()` is not supported.

### `AnimationEvent`

This component observes AnimationEvent fireing.

To receive AnimationEvent, Component needs to be attached to the same GameObject as Animator.

### `PlayableController`

This component will fire `PlayableDirector.Play()`.

If you do not specify instance of `PlayableDirector` will be obtained by `GameObject.GetComponent<PlayableDirector>()`

### `TimelineSignal`

This component observes Timeline Signals emission.

Specify `TimelineSignal.Dispatch()` in the destination method of the `UnityEngine.Timeline.SignalReceiver` component
