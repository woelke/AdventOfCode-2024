using System.Text;
using Utils;

namespace AdventOfCode
{
    public class Day20 : ISolver
    {
        public string PuzzleFolder => nameof(Day20);
        public (string Phase, int Idx)? PuzzleSelector => null;

        readonly record struct Pos(int X, int Y)
        {
            public Pos Left() => new(X - 1, Y);
            public Pos Top() => new(X, Y - 1);
            public Pos Right() => new(X + 1, Y);
            public Pos Bottom() => new(X, Y + 1);
        }

        (Dictionary<Pos, int> M, Pos S, Pos E) ParseInput(string[] lines)
        {
            Dictionary<Pos, int> m = [];
            var s = new Pos(0, 0);
            var e = new Pos(0, 0);

            foreach (var (line, y) in lines.WithIndex())
            {
                foreach (var (c, x) in line.WithIndex())
                {
                    var p = new Pos(x, y);
                    if (('.', 'S', 'E').Contains(c))
                        m[p] = int.MaxValue;

                    if (c == 'S')
                        s = p;
                    else if (c == 'E')
                        e = p;
                }
            }

            return (m, s, e);
        }

        void FillM(Dictionary<Pos, int> m, Pos start)
        {
            Queue<(Pos, int)> state = [];

            state.Enqueue((start, 0));

            while (!state.IsEmpty())
            {
                var (p, cc) = state.Dequeue();

                if (!m.TryGetValue(p, out var c) || c <= cc)
                    continue;

                m[p] = cc;

                foreach (var np in (p.Left(), p.Right(), p.Top(), p.Bottom()).AsEnumerable())
                    state.Enqueue((np, cc + 1));
            }
        }

        List<(Pos CheatP, int Saves)> GetShortCuts2ps(Dictionary<Pos, int> m)
        {
            List<(Pos CheatP, int Saves)> res = [];
            var xMax = m.Keys.Select(p => p.X).Max();
            var yMax = m.Keys.Select(p => p.Y).Max();

            foreach (var (x, y) in Enumerable.Range(1, xMax).Product(Enumerable.Range(1, yMax)))
            {
                var p = new Pos(x, y);

                if (m.ContainsKey(p))
                    continue;

                if (m.TryGetValue(p.Left(), out var lc) && m.TryGetValue(p.Right(), out var rc))
                    res.Add((p, Math.Abs(rc - lc) - 2));

                if (m.TryGetValue(p.Top(), out var tc) && m.TryGetValue(p.Bottom(), out var bc))
                    res.Add((p, Math.Abs(bc - tc) - 2));
            }

            return res;
        }

        IEnumerable<Pos> GetAllPossiblePossFromTaxiDistiance(Pos a, int dist)
        {
            foreach (var x in Enumerable.Range(0, dist + 1))
            {
                yield return new Pos(a.X + x, a.Y + (dist - x));
                yield return new Pos(a.X + x, a.Y - (dist - x));
                yield return new Pos(a.X - x, a.Y + (dist - x));
                yield return new Pos(a.X - x, a.Y - (dist - x));
            }
        }

        Dictionary<(Pos A, Pos B), int> GetTimeSaved(Dictionary<Pos, int> m, int cheatLength)
        {
            Dictionary<(Pos A, Pos B), int> res = [];
            var xMax = m.Keys.Select(p => p.X).Max();
            var yMax = m.Keys.Select(p => p.Y).Max();

            foreach (var a in Enumerable.Range(1, xMax).Product(Enumerable.Range(1, yMax)).Select(e => new Pos(e._1, e._2)))
            {
                if (!m.TryGetValue(a, out var ac))
                    continue;

                foreach (var b in GetAllPossiblePossFromTaxiDistiance(a, cheatLength))
                {

                    if (b.X <= 0 || b.X > xMax || b.Y <= 0 || b.Y > yMax)
                        continue;

                    if (!m.TryGetValue(b, out var bc))
                        continue;

                    if (bc <= ac)
                        continue;

                    var savings = bc - ac - cheatLength;
                    if (savings == 0)
                        continue;

                    res[(a, b)] = bc - ac - cheatLength;
                }
            }

            return res;
        }

        public string? CalcA(string[] lines, PuzzleInfo info)
        {
            var (m, s, e) = ParseInput(lines);
            FillM(m, s);

            var c = m[e];

            var saves = GetShortCuts2ps(m);
            var res = saves.GroupBy(e => e.Saves).Select(e => (e.Key, e.Size())).OrderBy(e => e.Item2).ToList();

            var threshold = 0;
            if ((info.Phase, info.Idx) == ("a", 1))
                threshold = 20;
            else
                threshold = 100;

            return res.Where(e => e.Item1 >= threshold).Select(e => e.Item2).Sum().ToString();
        }

        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            var (m, s, e) = ParseInput(lines);
            FillM(m, s);

            var c = m[e];

            var sum = 0;

            var minSavings = 0;
            var maxCheatLength = 0;

            if ((info.Phase, info.Idx) == ("b", 1))
            {
                minSavings = 50;
                maxCheatLength = 20;
            }
            else
            {
                minSavings = 100;
                maxCheatLength = 20;
            }

            foreach (var secs in Enumerable.Range(1, maxCheatLength))
            {
                var saves = GetTimeSaved(m, secs);
                sum += saves.Values.Where(e => e >= minSavings).Size();
            }

            return sum.ToString();
        }
    }
}
