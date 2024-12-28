using System.Text;
using Utils;

namespace AdventOfCode
{
    public class Day15A : ISolver
    {
        public string PuzzleFolder => nameof(Day15A);
        public (string Phase, int Idx)? PuzzleSelector => null;

        record Pos(int X, int Y)
        {
            public Pos Next(char d)
                => d switch
                {
                    '^' => new Pos(X, Y - 1),
                    '>' => new Pos(X + 1, Y),
                    'v' => new Pos(X, Y + 1),
                    '<' => new Pos(X - 1, Y),
                    _ => throw new NotImplementedException()
                };
        }

        record Field(List<List<char>> F)
        {
            public char Get(Pos p) => F[p.Y][p.X];
            public void Set(Pos p, char c) => F[p.Y][p.X] = c;

            public void Swap(Pos p1, Pos p2)
            {
                var c1 = Get(p1);
                var c2 = Get(p2);
                Set(p1, c2);
                Set(p2, c1);
            }

            public Pos FindRobot()
            {
                foreach (var (l, y) in F.WithIndex())
                {
                    foreach (var (c, x) in l.WithIndex())
                    {
                        if (c == '@')
                            return new(x, y);
                    }
                }

                return new(-1, -1);
            }

            public void Print()
            {
                var res = new StringBuilder();
                foreach (var l in F)
                {
                    foreach (var c in l)
                        res.Append(c);

                    res.AppendLine();
                }

                Console.WriteLine(res.ToString());
            }

            public int GPSSum()
            {
                var sum = 0;
                foreach (var (l, y) in F.WithIndex())
                {
                    foreach (var (c, x) in l.WithIndex())
                    {
                        if (c == 'O')
                            sum += 100 * y + x;
                    }
                }

                return sum;
            }
        }

        (Field F, List<char> Ds) ParseInput(string[] lines)
        {
            List<List<char>> f = [];
            List<char> ds = [];

            var s = 0;
            foreach (var line in lines)
            {
                if (line.IsEmpty())
                {
                    s = 1;
                    continue;
                }
                if (s == 0)
                    f.Add(line.ToCharArray().ToList());
                else
                    ds.AddRange(line.ToCharArray());
            }

            return (new(f), ds);
        }

        (bool Ok, Pos R) Move(Field f, Pos r, char d)
        {
            var nr = r.Next(d);
            var nc = f.Get(nr);

            if (nc == '.' || (nc == 'O' && Move(f, nr, d).Ok))
            {
                f.Swap(r, nr);
                return (true, nr);
            }
            else
                return (false, r);
        }

        public string? CalcA(string[] lines, PuzzleInfo info)
        {
            var (f, ds) = ParseInput(lines);
            var r = f.FindRobot();

            foreach (var d in ds)
            {
                r = Move(f, r, d).R;
            }

            return f.GPSSum().ToString();
        }


        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            return null;
        }
    }
}
