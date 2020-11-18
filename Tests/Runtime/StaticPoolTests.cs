using NUnit.Framework;

using UnityEngine;

using BBUnity;
using BBUnity.TestSupport;

public class StaticPoolTests {

    [Test]
    public void FindPoolDefinition_ShouldReturnAValidPoolDefinition() {
        TestUtilities.CreateThenDestroyGameObject((StaticPool pool) => {
            TestUtilities.CreateThenDestroyGameObject((GameObject obj) => {
                pool.AddPoolDefinition(new StaticPoolDefinition("Test Pool", obj,0, 1));
                StaticPoolDefinition definition = pool.FindPoolDefinition<StaticPoolDefinition>("Test Pool");

                Assert.NotNull(definition);
            });
        });
    }

    [Test]
    public void Spawn_ShouldSetThePosition() {
        TestUtilities.CreateThenDestroyGameObject((StaticPool pool) => {
            TestUtilities.CreateThenDestroyGameObject((GameObject obj) => {
                pool.AddPoolDefinition(new StaticPoolDefinition("Test Pool", obj,0, 1));

                PoolBehaviour newInstance = pool.Spawn("Test Pool", new Vector3(1.0f, 1.0f, 1.0f));

                Assert.NotNull(newInstance);
                UnityAssert.AreEqual(new Vector3(1.0f, 1.0f, 1.0f), newInstance.Position);
            });
        });
    }

    [Test]
    public void Spawn_ShouldSetTheRotation() {
        TestUtilities.CreateThenDestroyGameObject((StaticPool pool) => {
            TestUtilities.CreateThenDestroyGameObject((GameObject obj) => {
                pool.AddPoolDefinition(new StaticPoolDefinition("Test Pool", obj,0, 1));

                PoolBehaviour newInstance = pool.Spawn("Test Pool", Vector3.zero, Quaternion.identity);

                Assert.NotNull(newInstance);
                UnityAssert.AreEqual(Quaternion.identity, newInstance.Rotation);
            });
        });
    }

    [Test]
    public void Spawn_ShouldSetTheScale() {
        TestUtilities.CreateThenDestroyGameObject((StaticPool pool) => {
            TestUtilities.CreateThenDestroyGameObject((GameObject obj) => {
                pool.AddPoolDefinition(new StaticPoolDefinition("Test Pool", obj,0, 1));

                PoolBehaviour newInstance = pool.Spawn("Test Pool", Vector3.zero, Quaternion.identity, Vector3.one);

                Assert.NotNull(newInstance);
                UnityAssert.AreEqual(Vector3.one, newInstance.LocalScale);
            });
        });
    }
}
