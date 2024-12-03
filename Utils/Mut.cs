using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class Mut<T>(T val)
    {
        public T Val { get; set; } = val;
    }

    public static class Mut
    {
        public static Mut<T> From<T>(T val) => new(val);
    }
}
