using Utils;

namespace TestSuite
{
    [TestClass]
    public class IEnumerableTests
    {

        private IEnumerable<int> GetValues()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }

        [TestMethod]
        public void TestReuseIEnumerable()
        {
            var vals = GetValues();
            Assert.AreEqual(1, vals.First());
            Assert.AreEqual(1, vals.First());
            Assert.AreEqual(2, vals.Skip(1).First());
            Assert.AreEqual(2, vals.Skip(1).First());
        }
    }
}