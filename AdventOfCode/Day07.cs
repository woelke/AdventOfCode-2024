using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace AdventOfCode
{
    public class Day07 : ISolver
    {
        public string PuzzleFolder => nameof(Day07);
        public (string Phase, int Idx)? PuzzleSelector => null;

        bool IsOkA(Int128 goal, Int128 current, List<Int128> vals)
        {
            if (vals.Count == 0 && goal == current)
                return true;
            else if (current > goal || vals.Count == 0)
                return false;

            if (IsOkA(goal, current + vals[0], vals.Skip(1).ToList()))
                return true;
            else if (IsOkA(goal, current * vals[0], vals.Skip(1).ToList()))
                return true;
            else
                return false;
        }

        public string? CalcA(string[] lines, PuzzleInfo info)
        {
            var calcs = lines.Select(l => l.Split(':').To(e => (Int128.Parse(e[0].Trim()), e[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(f => Int128.Parse(f.Trim())))));

            return calcs.Select(e => IsOkA(e.Item1, 0, e.Item2.ToList()) ? e.Item1 : 0).Sum().ToString();
        }

        bool IsOkB(Int128 goal, Int128 current, List<Int128> vals)
        {
            if (vals.Count == 0 && goal == current)
                return true;
            else if (current > goal || vals.Count == 0)
                return false;

            if (IsOkB(goal, current + vals[0], vals.Skip(1).ToList()))
                return true;
            else if (IsOkB(goal, current * vals[0], vals.Skip(1).ToList()))
                return true;
            else if (IsOkB(goal, Int128.Parse($"{current}{vals[0]}"), vals.Skip(1).ToList()))
                return true;
            else
                return false;
        }


        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            var calcs = lines.Select(l => l.Split(':').To(e => (Int128.Parse(e[0].Trim()), e[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(f => Int128.Parse(f.Trim())))));

            return calcs.Select(e => IsOkB(e.Item1, 0, e.Item2.ToList()) ? e.Item1 : 0).Sum().ToString();
        }
    }
}
