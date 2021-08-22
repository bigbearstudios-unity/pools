using NUnit.Framework;

using BBUnity;
using BBUnity.Pools.Internal;
using BBUnity.TestSupport;

namespace Internal {
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
}
