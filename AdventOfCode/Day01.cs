﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day01 : ISolver
    {
        public string PuzzleFolder => nameof(Day01);
        public (string Phase, int Idx)? PuzzleSelector => null;

        public string? CalcA(string[] lines, PuzzleInfo info)
        {
            var raw = lines.Select(e => e.Split("   ").Select(s => Convert.ToInt32(s)));
            var l = raw.Select(e => e.First()).Order();
            var r = raw.Select(e => e.Last()).Order();
            var res = l.Zip(r).Select(e => Math.Abs(e.First - e.Second)).Sum();
            return res.ToString();
        }

        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            var raw = lines.Select(e => e.Split("   ").Select(s => Convert.ToInt32(s)));
            var ls = raw.Select(e => e.First());
            var rs = raw.Select(e => e.Last());

            var res = ls.Select(l => l *rs.Count(r => r == l)).Sum();
            return res.ToString();
        }
    }
}
