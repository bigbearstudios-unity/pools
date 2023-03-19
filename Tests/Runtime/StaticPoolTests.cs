using NUnit.Framework;

using UnityEngine;

using BBUnity.Pools;
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
}
