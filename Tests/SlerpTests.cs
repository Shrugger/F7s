using F7s.Utility;
using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double;

namespace Tests;
public class SlerpTests {


    [Test]
    public void TestOppositePoles () {
        Vector3 n = new Vector3(0, 1, 0);
        Vector3 s = new Vector3(0, -1, 0);
        Vector3 equator = Mathematik.Slerp(n, s, 0.5f);
        Vector3 expected = new Vector3(0, 0, 0);
        Assert.IsTrue(Mathematik.ApproximatelyEqual(expected, equator));
    }

    [Test]
    public void TestIcosahedraProblems () {
        Vector3 n = new Vector3(1, 1.6f, 0);
        Vector3 s = new Vector3(1, 1.6f, -1);
        Vector3 center = Mathematik.Slerp(n, s, 0.5f);
        Vector3 expected = new Vector3(0.448101521f, 1.54237938f, -0.21842736f);
        Assert.IsTrue(Mathematik.ApproximatelyEqual(expected, center));
    }
}
