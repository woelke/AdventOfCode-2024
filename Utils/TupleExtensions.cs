using System.Runtime.CompilerServices;

namespace Utils
{
    // TODO SWO write some unit tests for these extenstions
    public static class TupleExtentions
    {
        // RightPop
        public static (T _1, T _2) RightPop<T>(this (T _1, T _2, T _3) s)
            => (s._1, s._2);

        public static (T _1, T _2, T _3) RightPop<T>(this (T _1, T _2, T _3, T _4) s)
            => (s._1, s._2, s._3);

        // Append
        public static (T _1, T _2, T _3) Append<T>(this (T _1, T _2) s, T x)
            => (s._1, s._2, x);

        public static (T _1, T _2, T _3, T _4) Append<T>(this (T _1, T _2, T _3) s, T x)
            => (s._1, s._2, s._3, x);

        // Contains
        public static bool Contains<T>(this (T _1, T _2) s, T x, Func<T, T, bool> p)
            => p(s._1, x) || p(s._2, x);

        public static bool Contains<T>(this (T _1, T _2) s, T x) where T : IEquatable<T>
            => s.Contains(x, (y, x) => y.Equals(x));

        public static bool Contains<T>(this (T _1, T _2, T _3) s, T x, Func<T, T, bool> p)
            => s.RightPop().Contains(x, p) || p(s._3, x);

        public static bool Contains<T>(this (T _1, T _2, T _3) s, T x) where T : IEquatable<T>
            => s.Contains(x, (y, x) => y.Equals(x));

        public static bool Contains<T>(this (T _1, T _2, T _3, T _4) s, T x, Func<T, T, bool> p)
            => s.RightPop().Contains(x, p) || p(s._4, x);

        public static bool Contains<T>(this (T _1, T _2, T _3, T _4) s, T x) where T : IEquatable<T>
            => s.Contains(x, (y, x) => y.Equals(x));

        // Sum
        public static int Sum(this (int _1, int _2) s)
            => s._1 + s._2;

        public static int Sum(this (int _1, int _2, int _3) s)
            => s.RightPop().Sum() + s._3;

        public static int Sum(this (int _1, int _2, int _3, int _4) s)
            => s.RightPop().Sum() + s._4;

        // Select
        public static (U _1, U _2) Select<T, U>(this (T _1, T _2) s, Func<T, U> c)
            => (c(s._1), c(s._2));

        public static (U _1, U _2, U _3) Select<T, U>(this (T _1, T _2, T _3) s, Func<T, U> c)
            => s.RightPop().Select(c).Append(c(s._3));

        public static (U _1, U _2, U _3, U _4) Select<T, U>(this (T _1, T _2, T _3, T _4) s, Func<T, U> c)
            => s.RightPop().Select(c).Append(c(s._4));
    }
}
