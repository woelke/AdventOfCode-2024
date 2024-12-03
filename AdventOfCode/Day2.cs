using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace AdventOfCode
{
    public class Day2 : ISolver
    {
        public int Day => 2;

        private bool IsReportSave(IEnumerable<int> l)
        {
            var increase = l.First() < l.Second();
            return l.Zip(l.Skip(1)).Select(e => (e.Second - e.First) switch
            {
                (>= 1) and (<= 3) => increase == true,
                (<= -1) and (>= -3) => increase == false,
                _ => false
            }).All(b => b);
        }

        private bool IsAnySubReportSave(IEnumerable<int> l)
            => Enumerable.Range(0, l.Count())
                .Select(idx => l.WithIndex().Where(e => e.Idx != idx).Select(e => e.Item))
                .Select(m => IsReportSave(m))
                .Any(e => e);

        public string? CalcA(string[] lines)
            => lines
                .Select(l => l.Split(" ").Select(v => Convert.ToInt32(v)))
                .Select(IsReportSave)
                .Count(v => v)
                .ToString();

        public string? CalcB(string[] lines)
            => lines
                .Select(l => l.Split(" ").Select(v => Convert.ToInt32(v)))
                .Select(l => IsReportSave(l) || IsAnySubReportSave(l))
                .Count(v => v)
                .ToString();
    }
}
