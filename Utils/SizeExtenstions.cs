using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class SizeExtenstions
    {
        public static int Size<T>(this IEnumerable<T> self) => self.Count();
        public static int Size<T>(this List<T> self) => self.Count;
        public static int Size<T>(this Queue<T> self) => self.Count;
        public static int Size<T, U>(this Dictionary<T, U> self) where T : notnull => self.Count;
        public static int Size<T, U>(this HashSet<T> self) => self.Count;
        public static int Size<T, U>(this string self) => self.Length;

        public static bool IsEmpty<T>(this IEnumerable<T> self) => self.Size() == 0;
        public static bool IsEmpty<T>(this List<T> self) => self.Size() == 0;
        public static bool IsEmpty<T>(this Queue<T> self) => self.Size() == 0;
        public static bool IsEmpty<T, U>(this Dictionary<T, U> self) where T : notnull => self.Size() == 0;
        public static bool IsEmpty<T>(this HashSet<T> self) => self.Size() == 0;
        public static bool IsEmpty(this string self) => self.Size() == 0;
    }
}
