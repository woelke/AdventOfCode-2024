using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Utils
{
    public static class TupleExtentions
    {
        // RightPop
        public static (T _1, T _2) RightPop<T>(this (T _1, T _2, T _3) s)
            => (s._1, s._2);

        public static (T _1, T _2, T _3) RightPop<T>(this (T _1, T _2, T _3, T _4) s)
            => (s._1, s._2, s._3);

        public static (T _1, T _2, T _3, T _4) RightPop<T>(this (T _1, T _2, T _3, T _4, T _5) s)
            => (s._1, s._2, s._3, s._4);

        // Append
        public static (T _1, T _2, T _3) Append<T>(this (T _1, T _2) s, T x)
            => (s._1, s._2, x);

        public static (T _1, T _2, T _3, T _4) Append<T>(this (T _1, T _2, T _3) s, T x)
            => (s._1, s._2, s._3, x);

        public static (T _1, T _2, T _3, T _4, T _5) Append<T>(this (T _1, T _2, T _3, T _4) s, T x)
            => (s._1, s._2, s._3, s._4, x);

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

        public static (U _1, U _2, U _3, U _4, U _5) Select<T, U>(this (T _1, T _2, T _3, T _4, T _5) s, Func<T, U> c)
            => s.RightPop().Select(c).Append(c(s._5));

        // To
        public static R To<T, R>(this (T _1, T _2) self, Func<(T _1, T _2), R> fun)
            => fun(self);

        public static R To<T, R>(this (T _1, T _2, T _3) self, Func<(T _1, T _2, T _3), R> fun)
            => fun(self);

        public static R To<T, R>(this (T _1, T _2, T _3, T _4) self, Func<(T _1, T _2, T _3, T _4), R> fun)
            => fun(self);


        // AsEnumeralbe
        public static IEnumerable<T> AsEnumerable<T>(this (T _1, T _2) s)
        {
            yield return s._1;
            yield return s._2;
        }

        public static IEnumerable<T> AsEnumerable<T>(this (T _1, T _2, T _3) s)
            => s.RightPop().AsEnumerable().Append(s._3);

        public static IEnumerable<T> AsEnumerable<T>(this (T _1, T _2, T _3, T _4) s)
            => s.RightPop().AsEnumerable().Append(s._4);

        public static IEnumerable<T> AsEnumerable<T>(this (T _1, T _2, T _3, T _4, T _5) s)
            => s.RightPop().AsEnumerable().Append(s._5);

        // AsTuble
        private static (T _1, T _2) AsTuple2<T>(this IEnumerator<T> s)
        {
            if (!s.MoveNext())
                throw new IndexOutOfRangeException();

            var _1 = s.Current;

            if (!s.MoveNext())
                throw new IndexOutOfRangeException();

            return (_1, s.Current);
        }

        private static (T _1, T _2, T _3) AsTuple3<T>(this IEnumerator<T> s)
        {
            var t = s.AsTuple2();

            if (!s.MoveNext())
                throw new IndexOutOfRangeException();

            return t.Append(s.Current);
        }

        private static (T _1, T _2, T _3, T _4) AsTuple4<T>(this IEnumerator<T> s)
        {
            var t = s.AsTuple3();

            if (!s.MoveNext())
                throw new IndexOutOfRangeException();

            return t.Append(s.Current);
        }

        public static (T _1, T _2) AsTuple2<T>(this IEnumerable<T> s)
            => AsTuple2(s.GetEnumerator());

        public static (T _1, T _2, T _3) AsTuple3<T>(this IEnumerable<T> s)
            => AsTuple3(s.GetEnumerator());

        public static (T _1, T _2, T _3, T _4) AsTuple4<T>(this IEnumerable<T> s)
            => AsTuple4(s.GetEnumerator());

        // Flatten
        public static (T _1, T _2, T _3, T _4, T _5) Flatten<T>(this ((T _1, T _2, T _3) _1, T _2, T _3) s)
            => s._1.Append(s._2).Append(s._3);

    }
}
