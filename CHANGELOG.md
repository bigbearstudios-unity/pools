# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2020-11-08

### Added

- StaticPool
- TimedPool

## [1.0.1] - 2020-11-08

### Changed

- Upgraded to BBTestSupport 0.3.0, UnityAssert

## [2.0.0] - 2022-02-04

### Added

- Ability to use 'inactive' GameObjects
- Full callback functionality via events or IPoolBehaviour
- Added multiple .Spawn method which take positions, scaling, beforeSpawn and afterSpawn actions
- Added new FindPoolDefinition which are non-generic and return the type they are associated with.

### Changed

- Moved the BasePool, BasePoolDefinition to the .Internal namespace
- Changed Tooltips on all properties

### Removed

- Custom editor functionality. No longer required on all newer versions of Unity.

## [3.0.0] - 2023-03-19

### Changed

- Moved all non namespaced classes into the BBUnity.Pools namespace
