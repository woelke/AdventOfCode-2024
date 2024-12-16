using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace AdventOfCode
{
    public class Day08 : ISolver
    {
        public string PuzzleFolder => nameof(Day08);
        public (string Phase, int Idx)? PuzzleSelector => null;

        public Dictionary<char, List<(int X, int Y)>> ParseInput(string[] lines)
        {
            Dictionary<char, List<(int X, int Y)>> res = [];

            foreach (var (line, y) in lines.WithIndex())
            {
                foreach (var (c, x) in line.WithIndex())
                {
                    if (c == '.')
                        continue;

                    if (res.TryGetValue(c, out var value))
                        value.Add((x, y));
                    else
                        res[c] = [(x, y)];

                }
            }

            return res;
        }

        public (int X, int Y) CalcAntinode((int X, int Y) p1, (int X, int Y) p2)
            => (p1.X + (p1.X - p2.X), p1.Y + (p1.Y - p2.Y));

        public string? CalcA(string[] lines)
        {
            var map = ParseInput(lines);

            HashSet<(int X, int Y)> res = [];

            foreach (var (c, ps) in map)
            {
                foreach (var (p1, p2) in ps.Product(ps).Where(e => e.Item1 != e.Item2))
                {
                    var (x, y) = CalcAntinode(p1, p2);

                    if (0 <= y && y < lines.Length && 0 <= x && x < lines[0].Length)
                        res.Add((x, y));
                }
            }

            return res.Count.ToString();
        }

        IEnumerable<(int X, int Y)> CalcAntinodes((int X, int Y) p1, (int X, int Y) p2, (int X, int Y) fieldSize)
        {
            yield return (p1.X, p1.Y);
            yield return (p2.X, p2.Y);

            foreach (var f in Enumerable.Range(1, int.MaxValue))
            {
                var (x, y) = (p1.X + f * (p1.X - p2.X), p1.Y + f * (p1.Y - p2.Y));
                if (x < 0 || y < 0 || x >= fieldSize.X || y >= fieldSize.Y)
                    yield break;

                yield return (x, y);
            }
        }

        public string? CalcB(string[] lines)
        {
            var map = ParseInput(lines);

            HashSet<(int X, int Y)> res = [];

            foreach (var (c, ps) in map)
            {
                foreach (var (p1, p2) in ps.Product(ps).Where(e => e.Item1 != e.Item2))
                {
                    foreach (var (x, y) in CalcAntinodes(p1, p2, (lines[0].Length, lines.Length)))
                        res.Add((x, y));
                }
            }

            return res.Count.ToString();
        }
    }
}
