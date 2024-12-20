using Utils;

namespace AdventOfCode
{
    public class Day13 : ISolver
    {
        public string PuzzleFolder => nameof(Day13);
        public (string Phase, int Idx)? PuzzleSelector => null;

        record struct Eq(long C, long A, long B)
        {
            public readonly Eq ForB() => new Eq(C + 10000000000000, A, B);
        }


        IEnumerable<(Eq V, Eq W)> ParseInput(string[] lines)
        {
            long s = 0;
            long xC = 0, xA = 0, xB = 0;
            long yC = 0, yA = 0, yB = 0;

            foreach (var line in lines)
            {
                if (s == 0)
                {
                    (xA, yA) = line
                        .Split(", ")
                        .Select(e => e.Split('+')[1])
                        .To(e => (Convert.ToInt32(e.First()), Convert.ToInt32(e.Second())));
                    s++;
                }
                else if (s == 1)
                {
                    (xB, yB) = line
                        .Split(", ")
                        .Select(e => e.Split('+')[1])
                        .To(e => (Convert.ToInt32(e.First()), Convert.ToInt32(e.Second())));
                    s++;
                }
                else if (s == 2)
                {
                    (xC, yC) = line
                        .Split(", ")
                        .Select(e => e.Split('=')[1])
                        .To(e => (Convert.ToInt32(e.First()), Convert.ToInt32(e.Second())));
                    yield return (new(xC, xA, xB), new(yC, yA, yB));

                    s++;
                }
                else if (s == 3)
                    s = 0;
            }
        }

        (long A, long B) GetSolution(Eq v, Eq w)
        {
            var x = (v.B * w.C - w.B * v.C) / (v.B * w.A - w.B * v.A);
            var y = (v.C - (v.A * x)) / v.B;

            if ((v.C == v.A * x + v.B * y) && (w.C == w.A * x + w.B * y))
                return (x, y);
            else
                return (0, 0);
        }


        public string? CalcA(string[] lines, PuzzleInfo info)
            => ParseInput(lines)
                .Select(e => GetSolution(e.V, e.W).To(s => s._1 * 3L + s._2))
                .Sum()
                .ToString();

        public string? CalcB(string[] lines, PuzzleInfo info)
            => ParseInput(lines)
                .Select(e => (V: e.V.ForB(), W: e.W.ForB()))
                .Select(e => GetSolution(e.V, e.W).To(s => s._1 * 3L + s._2))
                .Sum()
                .ToString();
    }
}
