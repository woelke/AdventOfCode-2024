using Utils;

namespace AdventOfCode
{
    public class Day10 : ISolver
    {
        public string PuzzleFolder => nameof(Day10);
        public (string Phase, int Idx)? PuzzleSelector => null;

        public List<List<int>> ToMap(string[] lines)
            => lines.Select(l => l.Select(e => e == '.' ? 99 : Convert.ToInt32(e.ToString())).ToList()).ToList();

        HashSet<(int X, int Y)> FindScores(List<List<int>> map, (int X, int Y) cPos, int lHeight)
        {
            if (cPos.X < 0 || cPos.X >= map[0].Count || cPos.Y < 0 || cPos.Y >= map.Count)
                return [];

            var cHeight = map[cPos.Y][cPos.X];

            if (lHeight + 1 != cHeight)
                return [];

            if (cHeight == 9)
                return [(cPos)];

            return [
                .. FindScores(map, (cPos.X + 1, cPos.Y), cHeight),
                .. FindScores(map, (cPos.X, cPos.Y + 1), cHeight),
                .. FindScores(map, (cPos.X - 1, cPos.Y), cHeight),
                .. FindScores(map, (cPos.X , cPos.Y - 1), cHeight),
            ];
        }

        IEnumerable<(int X, int Y)> GetTrailHeads(List<List<int>> map)
        {
            foreach (var (l, yi) in map.WithIndex())
            {
                foreach (var (h, xi) in l.WithIndex())
                {
                    if (h == 0)
                        yield return (xi, yi);
                }
            }
        }

        public string? CalcA(string[] lines, PuzzleInfo info)
        {
            var map = ToMap(lines);

            List<(int X, int Y)> res = [];

            foreach (var (x, y) in GetTrailHeads(map))
                res.AddRange(FindScores(map, (x, y), -1));

            return res.Count.ToString();
        }

        public int GetRating(List<List<int>> map, (int X, int Y) cPos, int lHeight)
        {
            if (cPos.X < 0 || cPos.X >= map[0].Count || cPos.Y < 0 || cPos.Y >= map.Count)
                return 0;

            var cHeight = map[cPos.Y][cPos.X];

            if (lHeight + 1 != cHeight)
                return 0;

            if (cHeight == 9)
                return 1;

            return GetRating(map, (cPos.X + 1, cPos.Y), cHeight)
                + GetRating(map, (cPos.X, cPos.Y + 1), cHeight)
                + GetRating(map, (cPos.X - 1, cPos.Y), cHeight)
                + GetRating(map, (cPos.X, cPos.Y - 1), cHeight);
        }

        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            var map = ToMap(lines);

            var sum = 0;

            foreach (var (x, y) in GetTrailHeads(map))
                sum += GetRating(map, (x, y), -1);

            return sum.ToString();
        }
    }
}
