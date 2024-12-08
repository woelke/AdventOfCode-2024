using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace AdventOfCode
{
    public class Day3 : ISolver
    {
        public int Day => 3;

        IEnumerable<(long A, long B)> GetMuls(string x)
        {
            var nums = "1234567890";
            var ss = "mul(a,b)";
            var i = 0;

            string a = "";
            string b = "";

            var idx = 0;
            while (idx < x.Length)
            {
                if (ss[i] == 'a' && nums.Contains(x[idx]))
                {
                    a += x[idx];
                    idx++;

                    if (!nums.Contains(x[idx]))
                        i++;
                }
                else if (ss[i] == 'b' && nums.Contains(x[idx]))
                {
                    b += x[idx];
                    idx++;

                    if (!nums.Contains(x[idx]))
                        i++;
                }
                else if (x[idx] == ss[i])
                {
                    if (ss[i] == ')')
                    {
                        //Console.WriteLine($"{a}, {b}");
                        yield return (Convert.ToInt64(a), Convert.ToInt64(b));
                        i = 0;
                        idx++;
                        a = "";
                        b = "";
                    }
                    else
                    {
                        i++;
                        idx++;
                    }
                }
                else
                {
                    i = 0;
                    idx++;
                    a = "";
                    b = "";
                }
            }
        }


        public string? CalcA(string[] lines)
            => lines.Select(l => GetMuls(l)).SelectMany(e => e).Select(e => e.A * e.B).Sum().ToString();


        IEnumerable<string> GetEnabled(string x)
        {
            var enabled = true;
            var nx = x;

            while(true)
            {
                if (enabled)
                {
                    var s = nx.Split("don't()", 2);
                    if (s.Length == 1)
                    {
                        yield return nx;
                        break;
                    }
                    else
                    {
                        yield return s[0];
                        nx = s[1];
                        enabled = false;
                    }
                }
                else
                {
                    var s = nx.Split("do()", 2);
                    if (s.Length == 1)
                        break;
                    else
                    {
                        nx = s[1];
                        enabled = true;
                    }
                }
            }
        }

        public string? CalcB(string[] lines)
            => GetEnabled(string.Join("", lines))
                .Select(e => GetMuls(e))
                .SelectMany(e => e)
                .Select(e => e.A * e.B)
                .Sum()
                .ToString();
    }
}
