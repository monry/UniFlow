# Changelog

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
