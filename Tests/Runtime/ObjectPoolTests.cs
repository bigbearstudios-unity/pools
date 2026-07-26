using System;
using System.Text.RegularExpressions;

using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

using BBUnity.TestSupport;

namespace BBUnity.Pools.Tests {
    public class ObjectPoolTests {

        private GameObject _prefab;
        private ObjectPool _pool;

        [SetUp]
        public void SetUp() {
            _prefab = TestUtilities.CreateGameObject(name: "Pooled Prefab Stand-in");
            _pool = TestUtilities.CreateGameObject<ObjectPool>(name: "Pool Under Test");
        }

        [TearDown]
        public void TearDown() {
            TestUtilities.DestroyRootObjectsInScene();
        }

        [Test]
        public void FindPoolReference_UnknownName_ReturnsNull() {
            Assert.IsNull(_pool.FindPoolReference("missing"));
        }

        [Test]
        public void FindPoolReference_UnknownNameWithRaiseError_Throws() {
            Assert.Throws<Exception>(() => _pool.FindPoolReference("missing", raiseError: true));
        }

        [Test]
        public void AddPoolReference_InvalidReference_Throws() {
            var invalidReference = new ObjectPoolReference("Invalid", null);

            Assert.Throws<Exception>(() => _pool.AddPoolReference(invalidReference));
        }

        [Test]
        public void AddPoolReference_ValidReference_IsFindableByName() {
            var reference = new ObjectPoolReference("Enemy", _prefab);

            _pool.AddPoolReference(reference);

            Assert.AreEqual(reference, _pool.FindPoolReference("Enemy"));
        }

        [Test]
        public void AddPoolReference_DuplicateName_ThrowsClearError() {
            _pool.AddPoolReference(new ObjectPoolReference("Enemy", _prefab));

            var ex = Assert.Throws<Exception>(() => _pool.AddPoolReference(new ObjectPoolReference("Enemy", _prefab)));
            StringAssert.Contains("Enemy", ex.Message);
        }

        [Test]
        public void Spawn_UnknownName_ReturnsNullAndLogsError() {
            LogAssert.Expect(LogType.Error, new Regex("no pool reference named 'missing'"));

            PoolBehaviour result = _pool.Spawn("missing");

            Assert.IsNull(result);
        }

        [Test]
        public void Spawn_KnownName_ReturnsInstance() {
            _pool.AddPoolReference(new ObjectPoolReference("Enemy", _prefab, startingSize: 1, maximumSize: 5));

            PoolBehaviour result = _pool.Spawn("Enemy");

            Assert.IsNotNull(result);
        }

        [Test]
        public void FindInScene_UnknownName_ReturnsNull() {
            Assert.IsNull(ObjectPool.FindInScene("Does Not Exist"));
        }

        [Test]
        public void FindInScene_KnownName_ReturnsThatPool() {
            Assert.AreEqual(_pool, ObjectPool.FindInScene("Pool Under Test"));
        }
    }
}
