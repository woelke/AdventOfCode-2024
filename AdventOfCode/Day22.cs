
using System.Numerics;
using Utils;

namespace AdventOfCode
{
    public class Day22 : ISolver
    {
        public string PuzzleFolder => nameof(Day22);
        public (string Phase, int Idx)? PuzzleSelector => null;

        static long Mix(long s, long v) => s ^ v;

        static long Prune(long s) => s % 16777216;

        static long Next(long s0)
        {
            var s1 = Prune(Mix(s0, s0 * 64));
            var s2 = Prune(Mix(s1, s1 / 32));
            return Prune(Mix(s2, s2 * 2048));
        }

        public string? CalcA(string[] lines, PuzzleInfo info)
            => lines
                .Select(long.Parse)
                .Select(s => Enumerable.Range(0, 2000).Aggregate(s, (ac, _) => Next(ac)))
                .Sum()
                .ToString();

        Dictionary<(int _1, int _2, int _3, int _4), int> GetSeq(long s0)
        {
            Dictionary<(int _1, int _2, int _3, int _4), int> res = [];

            var es = Enumerable
                .Range(0, 2000)
                .Aggregate(new List<long>() { s0 }, (ac, _) =>
                {
                    ac.Add(Next(ac[^1]));
                    return ac;
                });

            foreach (var l in es
                .Zip(es.Skip(1), es.Skip(2))
                .Zip(es.Skip(3), es.Skip(4))
                .Select(e => e.Flatten().AsEnumerable().Select(f => (int)(f % 10)).ToList()))
            {
                var k = l.Zip(l.Skip(1)).Select(e => e.Second - e.First).AsTuple4();
                var v = l[4];

                res.TryAdd(k, v);
            }

            return res;
        }

        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            var seqs = lines.Select(long.Parse).Select(GetSeq).ToList();

            Dictionary<(int _1, int _2, int _3, int _4), int> seqSums = [];

            foreach (var seq in seqs)
            {
                foreach (var (k, v) in seq)
                {
                    if (seqSums.TryGetValue(k, out var sum))
                        seqSums[k] = sum + v;
                    else
                        seqSums[k] = v;
                }
            }

            var res = seqSums.MaxBy(kv => kv.Value);
            return res.Value.ToString();
        }
    }
}
