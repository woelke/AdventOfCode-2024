using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace AdventOfCode
{
    public class Day4 : ISolver
    {
        public int Day => 4;

        IEnumerable<IEnumerable<(int X, int Y, char C)>> GetPosssXMas()
        {
            List<(int X, int Y, char C)> x = [(1, 1, 'M'), (2, 2, 'A'), (3, 3, 'S')];

            yield return x.Select(e => (e.X, 0, e.C));
            yield return x.Select(e => (e.X, e.Y, e.C));
            yield return x.Select(e => (e.X, -e.Y, e.C));

            yield return x.Select(e => (-e.X, 0, e.C));
            yield return x.Select(e => (-e.X, e.Y, e.C));
            yield return x.Select(e => (-e.X, -e.Y, e.C));

            yield return x.Select(e => (0, e.Y, e.C));
            yield return x.Select(e => (0, -e.Y, e.C));
        }

        public class Map(string[] lines)
        {
            public char? GetC(int x, int y)
            {
                if (x < 0 || y < 0)
                    return null;

                if (y >= lines.Length)
                    return null;

                var l = lines[y];

                if (x >= l.Length)
                    return null;

                return l[x];
            }
        }

        public string? CalcA(string[] lines)
        {
            var map = new Map(lines);

            var res = 0;

            foreach (var y in Enumerable.Range(0, lines.Length))
            {
                foreach (var x in Enumerable.Range(0, lines[y].Length))
                {
                    if (map.GetC(x, y) != 'X')
                        continue;

                    res += GetPosssXMas()
                        .Select(poss => poss.Select(pos => map.GetC(x + pos.X, y + pos.Y) == pos.C).All(e => e))
                        .Count(e => e);
                }
            }

            return res.ToString();
        }

        IEnumerable<IEnumerable<(int X, int Y, char C)>> GetPosssMas()
        {
            yield return [(-1, -1, 'M'), (1, 1, 'S'), (-1, 1, 'M'), (1, -1, 'S')];
            yield return [(-1, -1, 'M'), (1, 1, 'S'), (-1, 1, 'S'), (1, -1, 'M')];

            yield return [(-1, -1, 'S'), (1, 1, 'M'), (-1, 1, 'M'), (1, -1, 'S')];
            yield return [(-1, -1, 'S'), (1, 1, 'M'), (-1, 1, 'S'), (1, -1, 'M')];
        }


        public string? CalcB(string[] lines)
        {
            var map = new Map(lines);

            var res = 0;

            foreach (var y in Enumerable.Range(0, lines.Length))
            {
                foreach (var x in Enumerable.Range(0, lines[y].Length))
                {
                    if (map.GetC(x, y) != 'A')
                        continue;

                    res += GetPosssMas()
                        .Select(poss => poss.Select(pos => map.GetC(x + pos.X, y + pos.Y) == pos.C).All(e => e))
                        .Count(e => e);
                }
            }

            return res.ToString();
        }
    }
}
