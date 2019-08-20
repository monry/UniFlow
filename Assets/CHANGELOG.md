# Changelog

## [0.1.0-preview.8] - 2019-08-20

Improve inspector

### Improvements

* Remove `ActAsReceiver` parameter from Connector components

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
