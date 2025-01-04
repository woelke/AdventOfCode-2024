using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Utils
{
    public static class BigIntegerExtentions
    {
        public static long ToLong(this BigInteger self)
           => (long)self;

        public static int ToInt(this BigInteger self)
           => (int)self;
    }
}
