using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using Utils;

namespace AdventOfCode
{
    public class Day15B : ISolver
    {
        public string PuzzleFolder => nameof(Day15B);
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
                        if (c == '[')
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
                    f.Add(line.ToCharArray().Select<char, List<char>>(c => c switch
                    {
                        '#' => ['#', '#'],
                        '@' => ['@', '.'],
                        'O' => ['[', ']'],
                        '.' => ['.', '.'],
                        _ => ['?', '?']

                    }).SelectMany(e => e).ToList());
                else
                    ds.AddRange(line.ToCharArray());
            }

            return (new(f), ds);
        }

        (bool Ok, Pos R) MoveH(Field f, Pos r, char d)
        {
            var nr = r.Next(d);
            var nc = f.Get(nr);

            if (nc == '.'
                || ((nc == ']' || nc == '[') && MoveH(f, nr, d).Ok))
            {
                f.Swap(r, nr);
                return (true, nr);
            }
            else
                return (false, r);
        }

        (bool Ok, Pos R) MoveV(Field f, Pos r, char d, bool dryRun)
        {
            var nr = r.Next(d);
            var nc = f.Get(nr);

            if (nc == '.')
            {
                if (!dryRun)
                    f.Swap(r, nr);
                return (true, nr);
            }
            else if (nc == ']')
            {
                if (MoveV(f, nr, d, dryRun).Ok && MoveV(f, nr.Next('<'), d, dryRun).Ok)
                {
                    if (!dryRun)
                    {
                        f.Swap(r, nr);
                        f.Swap(r, nr.Next('<'));
                    }

                    return (true, nr);
                }
            }
            else if (nc == '[')
            {
                if (MoveV(f, nr, d, dryRun).Ok && MoveV(f, nr.Next('>'), d, dryRun).Ok)
                {
                    if (!dryRun)
                    {
                        f.Swap(r, nr);
                        f.Swap(r, nr.Next('>'));
                    }

                    return (true, nr);
                }
            }

            return (false, r);
        }

        (bool Ok, Pos R) Move(Field f, Pos r, char d)
        {
            if (('^', 'v').Contains(d))
            {
                var res = MoveV(f, r, d, true);
                if (res.Ok)
                    return MoveV(f, r, d, false);
                else
                    return res;
            }
            else
                return MoveH(f, r, d);
        }


        public string? CalcA(string[] lines, PuzzleInfo info)
            => null;

        public string? CalcB(string[] lines, PuzzleInfo info)
        {

            var (f, ds) = ParseInput(lines);
            var r = f.FindRobot();
            //f.Print();

            foreach (var d in ds)
            {
                r = Move(f, r, d).R;

                //Console.WriteLine($"direction: {d}");
                //f.Print();
            }

            return f.GPSSum().ToString();
        }
    }
}
