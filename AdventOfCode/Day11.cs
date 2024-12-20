using Utils;

namespace AdventOfCode
{
    public class Day11 : ISolver
    {
        const int Max = 5;

        public string PuzzleFolder => nameof(Day11);
        public (string Phase, int Idx)? PuzzleSelector => null;

        List<Int128> Blink(Int128 s)
        {
            if (s == 0)
                return [1];

            var ss = s.ToString();
            if (ss.Length % 2 == 0)
                return [Int128.Parse(ss.Substring(0, ss.Length / 2)), Int128.Parse(ss.Substring(ss.Length / 2))];

            checked
            {
                return [s * 2024];
            }
        }

        Dictionary<(Int128 S, int Bs), List<Int128 >> _cache = [];
        Dictionary<(Int128 S, int Bs), long> _counts = [];

        List<Int128> GetStonesUMax((Int128 S, int Bs) r)
        {
            if (_cache.TryGetValue(r, out var l))
                return l;

            List<Int128> res = [];

            if (r.Bs == 1)
                res = Blink(r.S);
            else
                foreach (var s in Blink(r.S))
                    res.AddRange(GetStonesUMax((s, r.Bs - 1)));

            _cache[r] = res;
            _counts[r] = res.Count;
            return res;
        }

        long CalcStoneCount((Int128 S, int Bs) r)
        {
            if (_counts.TryGetValue(r, out var count))
                return count;

            if (r.Bs <= Max)
            {
                var res = GetStonesUMax(r).Count;
                _counts[r] = res;
                return res;
            }

            var sum = 0L;

            checked
            {
                foreach (var s in GetStonesUMax((r.S, Max)))
                    sum += CalcStoneCount((s, r.Bs - Max));
            }

            _counts[r] = sum;

            return sum;
        }

        public string? CalcA(string[] lines, PuzzleInfo info)
        {
            var sum = 0L;

            foreach (var v in lines[0].Split(' ').Select(e => Convert.ToInt64(e)))
                sum += CalcStoneCount((v, 25));

            return sum.ToString();
        }

        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            var sum = 0L;

            foreach (var v in lines[0].Split(' ').Select(e => Convert.ToInt64(e)))
                sum += CalcStoneCount((v, 75));

            return sum.ToString();
        }
    }
}
