using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class MyMath
    {
        public static long Gcd(long a, long b)
        {
            if (b == 0)
                return a;
            if (a > b)
                return Gcd(b, a % b);
            else
                return Gcd(a, b % a);
        }
    }
}
