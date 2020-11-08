using NUnit.Framework;

using BBUnity;
using BBUnity.Pools;
using BBUnity.TestSupport;

public class BasePoolTests {

    [Test]
    public void Find_ShouldReturnAValidPool() {
        TestUtilities.CreateThenDestroyGameObject((StaticPool pool) => {
            pool.name = "Test Pool";
            
            BasePool foundPool = BasePool.Find<StaticPool>("Test Pool");
            Assert.NotNull(foundPool);
        });
    }
}
