﻿using NUnit.Framework;

using UnityEngine;

using BBUnity;
using BBUnity.Pools;
using BBUnity.TestSupport;

public class BasePoolDefinitionTests {

    [Test]
    public void Valid_ShouldBeValid_WithAPrefab() {
        TestUtilities.CreateThenDestroyGameObject((GameObject obj) => {
            BasePoolDefinition pool = new BasePoolDefinition(obj);
            Assert.True(pool.Valid);
        });
    }

    [Test]
    public void NumberOfInstances_ShouldReturnDefaultInstances() {
        TestUtilities.CreateThenDestroyGameObject((GameObject obj) => {
            BasePoolDefinition pool = new BasePoolDefinition(obj, 5);
            Assert.AreEqual(5, pool.NumberOfInstances);
        });
    }

    [Test]
    public void AllowGrowth_ShouldReturnFalse_WhenMaximumIsSetToZero() {
        TestUtilities.CreateThenDestroyGameObject((GameObject obj) => {
            BasePoolDefinition pool = new BasePoolDefinition(obj, 0, 1);
            Assert.True(pool.AllowGrowth);
        });
    }

    [Test]
    public void AllowGrowth_ShouldReturnTrue_WhenMaximumIsSetToZero() {
        TestUtilities.CreateThenDestroyGameObject((GameObject obj) => {
            BasePoolDefinition pool = new BasePoolDefinition(obj, 0, 0);
            Assert.False(pool.AllowGrowth);
        });
    }

    [Test]
    public void Spawn_ShouldSpawnAPrefab_WithAnAvalibleInstance() {
        TestUtilities.CreateThenDestroyGameObject((GameObject obj) => {
            BasePoolDefinition pool = new BasePoolDefinition(obj, 5, 6);

            PoolBehaviour newInstance = pool.Spawn();

            Assert.True(pool.AllowGrowth);
            Assert.NotNull(newInstance);
            Assert.AreEqual(5, pool.NumberOfInstances);
            Assert.AreEqual(4, pool.NumberOfAvalibleInstances);
        });

    }

    [Test]
    public void Spawn_ShouldSpawnAPrefab_WithNoAvalibleInstances_WhenAllowGrowthTrue() {
        TestUtilities.CreateThenDestroyGameObject((GameObject obj) => {
            BasePoolDefinition pool = new BasePoolDefinition(obj, 0, 1);

            Assert.AreEqual(0, pool.NumberOfInstances);
            Assert.True(pool.AllowGrowth);

            PoolBehaviour newInstance = pool.Spawn();

            Assert.False(pool.AllowGrowth);
            Assert.NotNull(newInstance);
            Assert.AreEqual(1, pool.NumberOfInstances);
        });
    }

    [Test]
    public void Spawn_ShouldNotSpawnAPrefab_WithNoAvalibleInstance() {
        TestUtilities.CreateThenDestroyGameObject((GameObject obj) => {
            BasePoolDefinition pool = new BasePoolDefinition(obj, 0, 0);

            PoolBehaviour newInstance = pool.Spawn();

            Assert.False(pool.AllowGrowth);
            Assert.Null(newInstance);
            Assert.AreEqual(0, pool.NumberOfInstances);
        });
    }

    [Test]
    public void Spawn_ShouldSetThePosition() {
        TestUtilities.CreateThenDestroyGameObject((GameObject obj) => {
            BasePoolDefinition pool = new BasePoolDefinition(obj, 0, 1);

            PoolBehaviour newInstance = pool.Spawn(new Vector3(1.0f, 1.0f, 1.0f));

            Assert.NotNull(newInstance);
            BBAssert.AreEqual(new Vector3(1.0f, 1.0f, 1.0f), newInstance.Position);
        });
    }

    [Test]
    public void Spawn_ShouldSetTheRotation() {
        TestUtilities.CreateThenDestroyGameObject((GameObject obj) => {
            BasePoolDefinition pool = new BasePoolDefinition(obj, 0, 1);

            PoolBehaviour newInstance = pool.Spawn(Vector3.zero, Quaternion.identity);

            Assert.NotNull(newInstance);
            BBAssert.AreEqual(Quaternion.identity, newInstance.Rotation);
        });
    }

    [Test]
    public void Spawn_ShouldSetTheScale() {
        TestUtilities.CreateThenDestroyGameObject((GameObject obj) => {
            BasePoolDefinition pool = new BasePoolDefinition(obj, 0, 1);

            PoolBehaviour newInstance = pool.Spawn(Vector3.zero, Quaternion.identity, Vector3.one);

            Assert.NotNull(newInstance);
            BBAssert.AreEqual(Vector3.one, newInstance.LocalScale);
        });
    }
}