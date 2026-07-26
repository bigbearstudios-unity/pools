using System.Text.RegularExpressions;

using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

using BBUnity.TestSupport;

namespace BBUnity.Pools.Tests {
    public class ObjectPoolReferenceTests {

        private GameObject _prefab;

        [SetUp]
        public void SetUp() {
            _prefab = TestUtilities.CreateGameObject(name: "Pooled Prefab Stand-in");
        }

        [TearDown]
        public void TearDown() {
            TestUtilities.DestroyRootObjectsInScene();
        }

        [Test]
        public void Valid_WithPrefab_IsTrue() {
            var reference = new ObjectPoolReference(_prefab);

            Assert.IsTrue(reference.Valid);
            Assert.IsFalse(reference.Invalid);
        }

        [Test]
        public void Valid_WithoutPrefab_IsFalse() {
            var reference = new ObjectPoolReference("Unnamed", null);

            Assert.IsFalse(reference.Valid);
            Assert.IsTrue(reference.Invalid);
        }

        [Test]
        public void Name_ExplicitlySet_ReturnsThatName() {
            var reference = new ObjectPoolReference("Explicit Name", _prefab);

            Assert.AreEqual("Explicit Name", reference.Name);
        }

        [Test]
        public void Name_Unset_FallsBackToPrefabName() {
            var reference = new ObjectPoolReference(_prefab);
            reference.SetName(null);

            Assert.AreEqual(_prefab.name, reference.Name);
        }

        [Test]
        public void Name_UnsetAndNoPrefab_DoesNotThrow() {
            var reference = new ObjectPoolReference("Placeholder", null);
            reference.SetName(null);

            string name = null;
            Assert.DoesNotThrow(() => name = reference.Name);
            Assert.IsNotNull(name);
        }

        [Test]
        public void SetMaximumSize_NonPositiveValue_LogsWarning() {
            var reference = new ObjectPoolReference(_prefab);

            LogAssert.Expect(LogType.Warning, new Regex("MaximumSize"));
            reference.SetMaximumSize(0);

            Assert.AreEqual(0, reference.MaximumSize);
        }

        [Test]
        public void SetMaximumSize_PositiveValue_DoesNotLog() {
            var reference = new ObjectPoolReference(_prefab);

            reference.SetMaximumSize(5);

            LogAssert.NoUnexpectedReceived();
            Assert.AreEqual(5, reference.MaximumSize);
        }

        [Test]
        public void AllowGrowth_BelowMaximumSize_IsTrue() {
            var reference = new ObjectPoolReference(_prefab, startingSize: 0, maximumSize: 2);
            reference.InitialiseInstances();

            Assert.IsTrue(reference.AllowGrowth);
        }

        [Test]
        public void AllowGrowth_AtMaximumSize_IsFalse() {
            var reference = new ObjectPoolReference(_prefab, startingSize: 2, maximumSize: 2);
            reference.InitialiseInstances();

            Assert.IsFalse(reference.AllowGrowth);
        }

        [Test]
        public void InitialiseInstances_FillsUpToStartingSize() {
            var reference = new ObjectPoolReference(_prefab, startingSize: 3, maximumSize: 10);

            reference.InitialiseInstances();

            Assert.AreEqual(3, reference.NumberOfInstances);
        }

        [Test]
        public void InitialiseInstances_CalledTwice_IsANoOp() {
            var reference = new ObjectPoolReference(_prefab, startingSize: 3, maximumSize: 10);
            reference.InitialiseInstances();

            reference.InitialiseInstances();

            Assert.AreEqual(3, reference.NumberOfInstances);
        }

        [Test]
        public void Spawn_WhenFullAndGrowthDisallowed_ReturnsNull() {
            var reference = new ObjectPoolReference(_prefab, startingSize: 1, maximumSize: 1);
            reference.InitialiseInstances();
            reference.Spawn(); // consume the only instance

            PoolBehaviour result = reference.Spawn();

            Assert.IsNull(result);
        }

        [Test]
        public void Spawn_ReusesInactiveInstanceBeforeGrowing() {
            var reference = new ObjectPoolReference(_prefab, startingSize: 1, maximumSize: 5);
            reference.InitialiseInstances();
            PoolBehaviour first = reference.Spawn();
            first.ReturnToPool();

            reference.Spawn();

            Assert.AreEqual(1, reference.NumberOfInstances, "Reusing the returned instance should not have created a new one");
        }

        [Test]
        public void ActiveAndInactiveCount_TrackSpawnedInstances() {
            var reference = new ObjectPoolReference(_prefab, startingSize: 2, maximumSize: 5);
            reference.InitialiseInstances();

            reference.Spawn();

            Assert.AreEqual(1, reference.ActiveCount);
            Assert.AreEqual(1, reference.InactiveCount);
        }

        [Test]
        public void HasGrownBeyondStartingSize_FalseUntilPoolGrows() {
            var reference = new ObjectPoolReference(_prefab, startingSize: 1, maximumSize: 5);
            reference.InitialiseInstances();

            Assert.IsFalse(reference.HasGrownBeyondStartingSize);

            reference.Spawn();
            reference.Spawn(); // second spawn forces growth past the starting size of 1

            Assert.IsTrue(reference.HasGrownBeyondStartingSize);
        }

        [Test]
        public void ClearAll_ReturnsEveryActiveInstance() {
            var reference = new ObjectPoolReference(_prefab, startingSize: 2, maximumSize: 5);
            reference.InitialiseInstances();
            reference.Spawn();
            reference.Spawn();

            reference.ClearAll();

            Assert.AreEqual(0, reference.ActiveCount);
            Assert.AreEqual(2, reference.InactiveCount);
        }

        [Test]
        public void RemoveInstance_DecreasesNumberOfInstances() {
            var reference = new ObjectPoolReference(_prefab, startingSize: 1, maximumSize: 5);
            reference.InitialiseInstances();
            PoolBehaviour instance = reference.Spawn();

            Object.DestroyImmediate(instance.gameObject);

            Assert.AreEqual(0, reference.NumberOfInstances);
        }
    }
}
