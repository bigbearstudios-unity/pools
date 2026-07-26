# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- `PoolBehaviour.ReturnToPool()` — explicit, named return-to-pool method (previously an undocumented convention of deactivating the GameObject directly).
- `PoolBehaviour.OnReturnEvent` / `IPoolBehaviour.OnReturn()` / overridable `PoolBehaviour.OnReturn()`, mirroring the existing OnCreate/OnSpawn callback pair.
- `ObjectPoolReference.ClearAll()` — returns every currently-active instance to the pool at once.
- `ObjectPoolReference` statistics: `ActiveCount`, `InactiveCount`, `HasGrownBeyondStartingSize`.
- `Tests/` assembly covering `ObjectPoolReference`, `ObjectPool`, and `PoolBehaviour`.

### Changed

- `ObjectPoolReference.RefreshInstances()` renamed to `InitialiseInstances()` — the old name implied a reset, but it only ever performs one-time setup.
- `ObjectPool`'s internal name→reference lookup now stores the `ObjectPoolReference` directly instead of a list index, removing an invariant that would have broken silently if reference removal is ever added.
- `SetMaximumSize()` (and the `MaximumSize` setter, which now routes through it) logs a warning for a non-positive value instead of silently accepting it; `ObjectPool.OnValidate()` re-checks this for values set directly via the inspector.
- `ObjectPool.FindInScene` now returns `null` when no pool is found instead of throwing, matching what callers already expected.
- `IPoolBehaviour` and `PoolBehaviour`'s events are now documented with guidance on which to prefer (component on the pooled prefab vs. external caller).
- README rewritten to describe the current `ObjectPool`/`ObjectPoolReference`/`PoolBehaviour` API — it previously documented a `StaticPool`/`TimedPool`/`StaticPoolDefinition` API that no longer exists in this codebase.

### Fixed

- `ObjectPool.Spawn()` no longer throws `NullReferenceException` for an unregistered pool reference name; logs an error and returns `null` instead.
- `PoolBehaviour.AddOnCreateEvent`/`AddOnSpawnEvent` now throw a clear `MissingComponentException` instead of a raw `NullReferenceException` when the target has no `PoolBehaviour` component.
- `ObjectPoolReference.Name` no longer throws `NullReferenceException` when both the name and the prefab are unset.
- `ObjectPool.AddPoolReference`/`SetupPoolDefinition` now reject a duplicate pool reference name with a clear error instead of letting a raw `ArgumentException` bubble up from the lookup dictionary.
- `PoolBehaviour.OnDestroy()` now removes the instance from its `ObjectPoolReference`'s tracking, so a destroyed (rather than returned) instance can no longer be mistaken for a reusable one.
- `Spawn()` (`ObjectPool` and `ObjectPoolReference`) now documents that it can return `null` when the pool is full and not allowed to grow.

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
- Removed all 'named pools'
- Renamed Pool to ObjectPool
- Updated to BBUnity Core 4.0.0

## [3.0.1] - 2024-11-22

# Changed

- Updated to use BBUnity Core 5.1.1
    - Changed to use Utilities.Create from BBUnity
