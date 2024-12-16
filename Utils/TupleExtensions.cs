namespace Utils
{
    // TODO SWO write some unit tests for these extenstions
    public static class TupleExtentions
    {
        public static bool Contains<T>(this (T _1, T _2) s, T x, Func<T, T, bool> p)
            => p(s._1, x) || p(s._2, x);

        public static bool Contains<T>(this (T _1, T _2) s, T x) where T : IEquatable<T>
            => s.Contains(x, (y, x) => y.Equals(x));

        public static bool Contains<T>(this (T _1, T _2, T _3) s, T x, Func<T, T, bool> p)
            => (s._1, s._2).Contains(x, p) || p(s._3, x);

        public static bool Contains<T>(this (T _1, T _2, T _3) s, T x) where T : IEquatable<T>
            => s.Contains(x, (y, x) => y.Equals(x));

        public static bool Contains<T>(this (T _1, T _2, T _3, T _4) s, T x, Func<T, T, bool> p)
            => (s._1, s._2, s._3).Contains(x, p) || p(s._4, x);

        public static bool Contains<T>(this (T _1, T _2, T _3, T _4) s, T x) where T : IEquatable<T>
            => s.Contains(x, (y, x) => y.Equals(x));

    }
}
