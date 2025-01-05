namespace Utils
{
    // TODO SWO write some unit tests for these extenstions
    public static class IEnumerableExtensions
    {
        public static IEnumerable<(T Item, int Idx)> WithIndex<T>(this IEnumerable<T> self)
           => self.Select((item, index) => (item, index));

        public static T Second<T>(this IEnumerable<T> self)
            => self.Skip(1).First();

        public static IEnumerable<T> PrintMe<T>(this IEnumerable<T> self, string prepend = "")
        {
            Console.WriteLine($"{prepend}{string.Join(", ", self)}");
            return self;
        }

        public static TResult To<TInput, TResult>(this IEnumerable<TInput> self, Func<IEnumerable<TInput>, TResult> fun)
            => fun(self);

        public static TResult To<TResult>(this string[] self, Func<string[], TResult> fun)
            => fun(self);

        public static IEnumerable<(T _1, U _2)> Product<T, U>(this IEnumerable<T> self, IEnumerable<U> other)
        {
            foreach (var t in self)
            {
                foreach (var u in other)
                {
                    yield return (t, u);
                }
            }
        }

        public static Int128 Sum(this IEnumerable<Int128> self)
        {
            Int128 sum = 0;
            foreach (var v in self)
                sum += v;

            return sum;
        }

        public static void Swap<T>(this List<T> list, int i, int j)
            => (list[i], list[j]) = (list[j], list[i]);

        public static T? FirstOrNull<T>(this IEnumerable<T> self, Func<T, bool> predicate) where T : class
        {
            foreach (T item in self)
            {
                if (predicate(item))
                    return item;
            }
            return null;
        }

        public static T? FirstOrNoValue<T>(this IEnumerable<T> self, Func<T, bool> predicate) where T : struct
        {
            foreach (T item in self)
            {
                if (predicate(item))
                    return item;
            }
            return null;
        }

        public static bool SequenceEqual<T>(this IEnumerable<T> left, IEnumerable<T> right, Func<T, T, bool> isEqualFun)
        {
            using (var l = left.GetEnumerator())
            using (var r = right.GetEnumerator())
            {
                while (true)
                {
                    var (leftOk, rightOk) = (l.MoveNext(), r.MoveNext());
                    if (leftOk && rightOk && isEqualFun(l.Current, r.Current))
                        continue;
                    else if (!leftOk && !rightOk)
                        return true;
                    else
                        return false;
                }
            }
        }

        public static bool SetEqual<T>(this IEnumerable<T> left, IEnumerable<T> right, Func<T, T, bool> isEqualFun)
        {
            var leftList = left.ToList();
            var rightList = right.ToList();
            var rightOk = rightList.Select(_ => false).ToArray();

            foreach (var l in leftList)
            {
                bool found = false;

                foreach (var (r, rIdx) in rightList.WithIndex())
                {
                    if (!isEqualFun(l, r))
                        continue;

                    found = true;
                    rightOk[rIdx] = true;
                    break;
                }

                if (!found)
                    return false;
            }

            foreach (var (r, rIdx) in rightList.WithIndex())
            {
                if (rightOk[rIdx])
                    continue;

                if (!leftList.Exists(left => isEqualFun(left, r)))
                    return false;
            }

            return true;
        }
    }
}
