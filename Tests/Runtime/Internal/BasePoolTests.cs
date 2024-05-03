using NUnit.Framework;

using BBUnity.Pools;
using BBUnity.Pools.Internal;
using BBUnity.TestSupport;

namespace Internal {
    public class BasePoolTests {

        [Test]
        public void Find_ShouldReturnAValidPool() {
            TestUtilities.CreateThenDestroyGameObject((StaticPool pool) => {
                pool.name = "Test Pool";
            
                BasePool foundPool = BasePool.FindInScene<StaticPool>("Test Pool");
                Assert.NotNull(foundPool);
            });
        }
    }
}
