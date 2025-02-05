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

        [TestMethod]
        public void TestTest()
        {
            List<int> l = [1, 2, 3, 4, 5];

            foreach (var i in Enumerable.Range(0, l.Size()))
            {
                List<int> l1 = [.. l[0..i], .. l[(i + 1)..]];
                Console.WriteLine($"i={i}; {(string.Join(',', l1))}");
            }
        }
    }
}