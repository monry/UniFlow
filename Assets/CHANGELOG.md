# Changelog

## [0.1.0-preview.44] - 2020-11-18

Add CurrentRuntimePlatformProvider

### Features

- #221 Add CurrentRuntimePlatformProvider / Thanks @milkcocoa !!

## [0.1.0-preview.43] - 2020-10-06

Fix upm dependencies

### Fixes

- #219 Fix Zenject dependencies

## [0.1.0-preview.24] - 2019-10-04

### Implementations

* Add `ObservableReceiver` to receive as `IObservable<T>`

### Fixes

* Fixed an issue that sometimes caused timeline event preparation to fail
* Corrected the field to be used

## [0.1.0-preview.23] - 2019-10-03

### Breaking Changes

* Revert timing of preparing to `Start()` instead of `Awake()`
    * Avoid running in non-active components

### Fixes

* Fix messaging about SimpleAnimation

### Tests

* Add tests for SimpleAnimation

## [0.1.0-preview.22] - 2019-10-02

### Breaking Changes

* Send `UniRx.Unit` into connecting flows instead of `Message` to omit data messaging

### Implementations

* Implement ValueExtractor
* Implement ValueCombiner

### Improvements

* Display current flow progress in UniFlow Graph
* Support connect ValueProviders in UniFlow Graph

## [0.1.0-preview.21] - 2019-09-20

### Implementations

* Implement ValueComparer
* Implement ValueProvider

### Fixes

* Fix observable
    * Split message correctly
    * Send `ObservableReceiverBase` message correctly
* Fix GraphView

## [0.1.0-preview.20] - 2019-09-17

### Changes

* Change timing of preparing to `Awake()` instead of `Start()`

### Fixes

* Fix some bugs in GraphView
* Change method interface to send latest message correctly

## [0.1.0-preview.19] - 2019-09-16

### Implementations

* Add `Toss` and `Receive` connector to split graph

### Destructive Changes

* Pass previous message to `IConnector.OnConnectAsObservable()`

### Fixes

* `SimpleAnimationEvent`: Waiting animation correctly

## [0.1.0-preview.18] - 2019-09-10

Fix editor bug

### Fixes

* Set TargetComponents correctly when connect from search window

## [0.1.0-preview.17] - 2019-09-09

Fix undo

### Fixes

* Register GameObject creation event to undo stack

## [0.1.0-preview.16] - 2019-09-09

Fix error in FlowGraph

### Fixes

* Fix NullReferenceException in FlowGraph
* Support to render `List<T>`

## [0.1.0-preview.15] - 2019-09-06

Fix build issue

### Fixes

* Avoid compile error on build

## [0.1.0-preview.14] - 2019-09-02

None

## [0.1.0-preview.13] - 2019-09-02

Tweak

### Changes

* Rename connectables

### Fixes

* Omit shared components
* Fix `IObservable.Subscribe()`

## [0.1.0-preview.12] - 2019-09-02

Tweak

### Fixes

* Remove debug code
    * Remove shortcut key
* Update README.md

## [0.1.0-preview.11] - 2019-09-02

Implement UniFlow Graph

### Features

* UniFlow Graph
    * Display connectables in Graph View
    * Edit connectables
* Add `IObservableReceiver`

### Fixes

* SimpleAnimationEvent has been destroyed but still trying to access

## [0.1.0-preview.10] - 2019-08-22

Tweak

### Changes

* Change some enum default values to unselected

## [0.1.0-preview.9] - 2019-08-22

Add connectors

### Features

* Add connectors
    * MoveParentTransform
    * RaycastTargetController
    * SimpleAnimationEvent

### Improvements

* Added Connector to load/unload scenes using `enum` values

### Fixes

* Fixed an issue where GameObject to which SimpleAnimation was attached was incorrect

## [0.1.0-preview.8] - 2019-08-20

Improve inspector

### Improvements

* Remove `ActAsReceiver` parameter from Connector components

### Fixes

* Fixed regression where some SerializeFields disappeared

## [0.1.0-preview.7] - 2019-08-19

Implement `TimeScaleController`, `KeyEvent`

### Features

* Add `Connector.TimeScaleController`
* Add `Connector.KeyEvent`
* Add `Receiver.Log`
* Add `ObservableReceiverBase`

### Improvements

* Make accessor to public

## [0.1.0-preview.6] - 2019-08-11

Add Components automatically

### Features

* Add components required by some connectors automatically

### Improvements

* Refactor codes

## [0.0.4-preview.5] - 2019-08-06

Refactor code

### Features

* Add Interval event

### Improvements

* Refactor codes
* Reverses reference direction between Publishers

## [0.0.3-preview.4] - 2019-08-06

Fix minor bugs

### Fixes

* Avoid `CS0649` warning
* Fix version conflicts in UniRx

## [0.0.2-preview.3] - 2019-08-06

Support many events

### Features

* Support Lifecycle events
* Support Physics/Physics2D events
* Support Camera, Transform, RectTransform events
* Support Audio events

### Improvements

* Connect events correctly
* Refresh EventMessages on re-invoke event

### Supports

* Supported Unity version bumped up to 2019.2

## [0.0.1] - 2019-07-21

Initial version

### Features

* Implements simple events
