using Utils;

namespace TestSuite
{
    [TestClass]
    public class MyMahtTests
    {
        [TestMethod]
        public void TestReuseIEnumerable()
        {
            Assert.AreEqual(1L, MyMath.Gcd(1L,1L));
            Assert.AreEqual(1L, MyMath.Gcd(2L,1L));
            Assert.AreEqual(1L, MyMath.Gcd(1L,2L));
            Assert.AreEqual(3L, MyMath.Gcd(12L,9L));
            Assert.AreEqual(3L, MyMath.Gcd(9L,12L));
            Assert.AreEqual(1L, MyMath.Gcd(11L,12L));
        }
    }
}
