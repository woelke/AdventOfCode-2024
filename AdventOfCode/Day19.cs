using System.Text;
using Utils;

namespace AdventOfCode
{
    public class Day19 : ISolver
    {
        public string PuzzleFolder => nameof(Day19);
        public (string Phase, int Idx)? PuzzleSelector => null;

        (HashSet<string> Ts, List<string> Ps) ParseInput(string[] lines)
        {
            var ps = lines[2..].ToList();
            var ts = lines[0].Split(", ").ToHashSet();

            return (ts, ps);
        }

        bool IsPossible(HashSet<string> ts, string p, int maxTsSize, Dictionary<string, bool> cache)
        {
            if (cache.TryGetValue(p, out var cachedResult))
                return cachedResult;

            if (p.IsEmpty())
                return true;

            foreach (var s in Enumerable.Range(1, Math.Min(maxTsSize, p.Size())).Reverse())
            {
                var subP = p[0..s];

                if (ts.Contains(subP) && IsPossible(ts, p[s..], maxTsSize, cache))
                {
                    cache[p] = true;
                    return true;
                }
            }

            cache[p] = false;
            return false;
        }

        long CountSolutions(HashSet<string> ts, string p, int maxTsSize, Dictionary<string, long> cache)
        {
            if (cache.TryGetValue(p, out var cachedResult))
                return cachedResult;

            if (p.IsEmpty())
                return 1;

            var sum = 0L;

            foreach (var s in Enumerable.Range(1, Math.Min(maxTsSize, p.Size())).Reverse())
            {
                var subP = p[0..s];

                if (ts.Contains(subP))
                    sum += CountSolutions(ts, p[s..], maxTsSize, cache);
            }

            cache[p] = sum;
            return sum;
        }

        public string? CalcA(string[] lines, PuzzleInfo info)
        {
            var (ts, ps) = ParseInput(lines);

            return ps
                .Select(p => (p, IsPossible(ts, p, ts.Max(t => t.Size()), [])))
                .Count(e => e.Item2)
                .ToString();
        }

        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            var (ts, ps) = ParseInput(lines);

            return ps
                .Select(p => (p, CountSolutions(ts, p, ts.Max(t => t.Size()), [])))
                .Select(e => e.Item2)
                .Sum()
                .ToString();
        }
    }
}
