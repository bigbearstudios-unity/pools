using System.Collections.Generic;

using UnityEngine;
using NUnit.Framework;

using BBUnity.TestSupport;

namespace BBUnity.Pools.Tests {
    public class PoolBehaviourTests {

        private class TrackingPoolBehaviour : PoolBehaviour, IPoolBehaviour {
            public readonly List<string> CallOrder = new List<string>();

            public override void OnCreate() { CallOrder.Add("virtual:OnCreate"); }
            public override void OnSpawn() { CallOrder.Add("virtual:OnSpawn"); }
            public override void OnReturn() { CallOrder.Add("virtual:OnReturn"); }

            void IPoolBehaviour.OnCreate(PoolBehaviour behaviour) { CallOrder.Add("interface:OnCreate"); }
            void IPoolBehaviour.OnSpawn(PoolBehaviour behaviour) { CallOrder.Add("interface:OnSpawn"); }
            void IPoolBehaviour.OnReturn(PoolBehaviour behaviour) { CallOrder.Add("interface:OnReturn"); }
        }

        private class TestMonoBehaviour : MonoBehaviour {}

        private GameObject _prefab;

        [SetUp]
        public void SetUp() {
            _prefab = TestUtilities.CreateGameObject<TrackingPoolBehaviour>(name: "Tracking Prefab Stand-in").gameObject;
        }

        [TearDown]
        public void TearDown() {
            TestUtilities.DestroyRootObjectsInScene();
        }

        private ObjectPoolReference CreateReference() {
            return new ObjectPoolReference(_prefab, startingSize: 1, maximumSize: 5);
        }

        [Test]
        public void Create_InvokesVirtualAndInterfaceOnCreate() {
            ObjectPoolReference reference = CreateReference();
            reference.InitialiseInstances();

            var instance = (TrackingPoolBehaviour)reference.Spawn();

            CollectionAssert.Contains(instance.CallOrder, "virtual:OnCreate");
            CollectionAssert.Contains(instance.CallOrder, "interface:OnCreate");
        }

        [Test]
        public void Spawn_InvokesVirtualAndInterfaceOnSpawn() {
            ObjectPoolReference reference = CreateReference();
            reference.InitialiseInstances();

            var instance = (TrackingPoolBehaviour)reference.Spawn();

            CollectionAssert.Contains(instance.CallOrder, "virtual:OnSpawn");
            CollectionAssert.Contains(instance.CallOrder, "interface:OnSpawn");
        }

        [Test]
        public void Spawn_ActivatesTheGameObject() {
            ObjectPoolReference reference = CreateReference();
            reference.InitialiseInstances();

            var instance = reference.Spawn();

            Assert.IsTrue(instance.gameObject.activeSelf);
        }

        [Test]
        public void ReturnToPool_InvokesOnReturnAndDeactivates() {
            ObjectPoolReference reference = CreateReference();
            reference.InitialiseInstances();
            var instance = (TrackingPoolBehaviour)reference.Spawn();

            instance.ReturnToPool();

            CollectionAssert.Contains(instance.CallOrder, "virtual:OnReturn");
            CollectionAssert.Contains(instance.CallOrder, "interface:OnReturn");
            Assert.IsFalse(instance.gameObject.activeSelf);
        }

        [Test]
        public void OnSpawnEvent_FiresWhenInstanceIsReused() {
            ObjectPoolReference reference = CreateReference();
            reference.InitialiseInstances();
            PoolBehaviour instance = reference.Spawn();
            instance.ReturnToPool();

            bool fired = false;
            instance.OnSpawnEvent += (_) => fired = true;

            reference.Spawn(); // only one instance exists, so this must reuse and re-fire OnSpawnEvent on it

            Assert.IsTrue(fired);
        }

        [Test]
        public void ReturnToPool_OnReturnEvent_Fires() {
            ObjectPoolReference reference = CreateReference();
            reference.InitialiseInstances();
            PoolBehaviour instance = reference.Spawn();

            bool fired = false;
            instance.OnReturnEvent += (_) => fired = true;

            instance.ReturnToPool();

            Assert.IsTrue(fired);
        }

        [Test]
        public void AddOnCreateEvent_MissingComponent_ThrowsMissingComponentException() {
            TestMonoBehaviour plainBehaviour = TestUtilities.CreateGameObject<TestMonoBehaviour>(name: "No PoolBehaviour Here");

            Assert.Throws<MissingComponentException>(() => PoolBehaviour.AddOnCreateEvent(plainBehaviour, (_) => {}));
        }

        [Test]
        public void AddOnSpawnEvent_MissingComponent_ThrowsMissingComponentException() {
            TestMonoBehaviour plainBehaviour = TestUtilities.CreateGameObject<TestMonoBehaviour>(name: "No PoolBehaviour Here");

            Assert.Throws<MissingComponentException>(() => PoolBehaviour.AddOnSpawnEvent(plainBehaviour, (_) => {}));
        }
    }
}
