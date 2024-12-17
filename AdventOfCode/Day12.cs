using Utils;

namespace AdventOfCode
{
    public class Day12 : ISolver
    {
        public string PuzzleFolder => nameof(Day12);
        public (string Phase, int Idx)? PuzzleSelector => null;

        HashSet<(int X, int Y)> GetArea(string[] lines, char c, (int X, int Y) pos, HashSet<(int X, int Y)> visited)
        {
            if (pos.X < 0 || pos.X >= lines[0].Length || pos.Y < 0 || pos.Y >= lines.Length)
                return visited;

            if (visited.Contains(pos))
                return visited;

            if (lines[pos.Y][pos.X] != c)
                return visited;

            visited.Add(pos);

            GetArea(lines, c, (pos.X + 1, pos.Y), visited);
            GetArea(lines, c, (pos.X, pos.Y + 1), visited);
            GetArea(lines, c, (pos.X - 1, pos.Y), visited);
            GetArea(lines, c, (pos.X, pos.Y - 1), visited);

            return visited;
        }

        Dictionary<char, List<HashSet<(int X, int Y)>>> GetAreass(string[] lines)
        {
            Dictionary<char, List<HashSet<(int X, int Y)>>> res = [];

            foreach (var (line, y) in lines.WithIndex())
            {
                foreach (var (c, x) in line.WithIndex())
                {
                    if (res.TryGetValue(c, out var sets))
                    {
                        if (sets.Select(s => s.Contains((x, y))).Any(e => e))
                            continue;
                    }

                    var area = GetArea(lines, c, (x, y), []);

                    if (sets is null)
                        res[c] = [area];
                    else
                        res[c].Add(area);
                }
            }

            return res;
        }

        int CalcPriceA(HashSet<(int X, int Y)> s)
        {
            var noFenceCount = s.Select(e
                => (s.Contains((e.X + 1, e.Y)),
                    s.Contains((e.X, e.Y + 1)),
                    s.Contains((e.X - 1, e.Y)),
                    s.Contains((e.X, e.Y - 1)))
                        .Select(b => b ? 1 : 0)
                        .Sum())
                .Sum();

            return s.Count * ((s.Count * 4) - noFenceCount);
        }

        public string? CalcA(string[] lines)
        {
            var areass = GetAreass(lines);

            var sum = 0;

            foreach (var (c, areas) in areass)
                sum += areas.Select(CalcPriceA).Sum();

            return sum.ToString();
        }

        int CountEdges(HashSet<(int X, int Y)> s)
        {
            Dictionary<(int X, int Y), List<int>> edgeCount = [];

            var addOrSet = (int edge, (int X, int Y) pos) =>
            {
                if (edgeCount.ContainsKey(pos))
                    edgeCount[pos].Add(edge);
                else
                    edgeCount[pos] = [edge];
            };

            foreach (var (x, y) in s)
            {
                addOrSet(1, (x, y));
                addOrSet(2, (x + 1, y));
                addOrSet(3, (x + 1, y + 1));
                addOrSet(4, (x, y + 1));
            }

            return edgeCount.Values.Select(v =>
            {
                if ((1, 3).Contains(v.Count))
                    return 1;

                if (v.Count == 2 && v.Contains(2) && v.Contains(4))
                    return 2;

                if (v.Count == 2 && v.Contains(3) && v.Contains(1))
                    return 2;

                return 0;
            }).Sum();
        }

        int CalcPriceB(HashSet<(int X, int Y)> s)
            => CountEdges(s) * s.Count;

        public string? CalcB(string[] lines)
        {
            var areass = GetAreass(lines);

            var sum = 0;

            foreach (var (c, areas) in areass)
                sum += areas.Select(CalcPriceB).Sum();

            return sum.ToString();
        }
    }
}
