using System.Security.Cryptography;
using System.Text;
using Utils;

namespace AdventOfCode
{
    public class Day21 : ISolver
    {
        public string PuzzleFolder => nameof(Day21);
        public (string Phase, int Idx)? PuzzleSelector => null;

        readonly record struct Pos(int X, int Y)
        {
            public Pos Left() => new(X - 1, Y);
            public Pos Top() => new(X, Y - 1);
            public Pos Right() => new(X + 1, Y);
            public Pos Bottom() => new(X, Y + 1);
        }

        record NumPad()
        {
            private static readonly List<List<char>> Bs = [
                [.. "789"],
                [.. "456"],
                [.. "123"],
                [.. "x0A"],
                ];

            private static char Get(Pos p) => Bs[p.Y][p.X];

            private static Pos ToPos(char b)
            {
                foreach (var (line, y) in Bs.WithIndex())
                {
                    foreach (var (c, x) in line.WithIndex())
                    {
                        if (b == c)
                            return new Pos(x, y);
                    }
                }

                throw new ArgumentException($"{b} not found");
            }

            private static IEnumerable<string> Paths(char a, char b)
            {
                var pa = ToPos(a);
                var pb = ToPos(b);

                Stack<List<(Pos P, char D)>> tasks = [];
                tasks.Push([(pa, 'x')]);

                while (!tasks.IsEmpty())
                {
                    var l = tasks.Pop();
                    var p = l[^1].P;

                    if (Get(p) == 'x')
                        continue;

                    if (p == pb)
                        yield return string.Join(null, l.Skip(1).Select(e => e.D).Append('A'));

                    var dh = pb.X - p.X;
                    if (dh > 0)
                        tasks.Push(l.Append((p.Right(), '>')).ToList());
                    else if (dh < 0)
                        tasks.Push(l.Append((p.Left(), '<')).ToList());

                    var dv = pb.Y - p.Y;
                    if (dv > 0)
                        tasks.Push(l.Append((p.Bottom(), 'v')).ToList());
                    else if (dv < 0)
                        tasks.Push(l.Append((p.Top(), '^')).ToList());
                }
            }

            public static IEnumerable<string> Paths(string input)
            {
                IEnumerable<string> ls = Paths('A', input.First());

                foreach (var (a, b) in input.Zip(input.Skip(1)))
                    ls = ls.Product(Paths(a, b)).Select(e => e._1 + e._2);

                return ls;
            }
        }

        record DirPad()
        {
            private static readonly List<List<char>> Bs = [
                [.. "x^A"],
                [.. "<v>"],
                ];

            private static List<char> Path(char a, char b)
            {
                List<char> res = (a, b) switch
                {
                    ('A', '^') => ['<'],
                    ('A', '>') => ['v'],
                    ('A', 'v') => ['v', '<'],
                    ('A', '<') => ['v', '<', '<'],

                    ('^', 'A') => ['>'],
                    ('^', '>') => ['>', 'v'],
                    ('^', 'v') => ['v'],
                    ('^', '<') => ['v', '<'],

                    ('>', 'A') => ['^'],
                    ('>', '^') => ['^', '<'],
                    ('>', 'v') => ['<'],
                    ('>', '<') => ['<', '<'],

                    ('v', 'A') => ['^', '>'],
                    ('v', '>') => ['>'],
                    ('v', '^') => ['^'],
                    ('v', '<') => ['<'],

                    ('<', 'A') => ['>', '>', '^'],
                    ('<', '>') => ['>', '>'],
                    ('<', '^') => ['>', '^'],
                    ('<', 'v') => ['>'],

                    (_, _) => []
                };

                res.Add('A');

                return res;
            }

            public string Path(string input)
            {
                List<char> res = Path('A', input.First());

                foreach (var (a, b) in input.Zip(input.Skip(1)))
                    res.AddRange(Path(a, b));

                return string.Join(null, res);
            }


            public string Path(string input, int it)
            {
                var res = input;

                foreach (var _ in Enumerable.Range(1, it))
                    res = Path(res);

                return res;
            }

            private Dictionary<(string Input, int It), Int128> cache = [];

            public Int128 PathSize(string input, int it)
            {
                Int128 sum = 0;

                foreach (var subInput in input.Split('A').Select(e => $"{e}A").SkipLast(1))
                {
                    if (cache.TryGetValue((subInput, it), out var cached))
                    {
                        sum += cached;
                        continue;
                    }

                    var temp = subInput;

                    foreach (var _ in Enumerable.Range(1, it))
                        temp = Path(temp);

                    cache[(subInput, it)] = temp.Size();

                    sum += temp.Size();
                }
                return sum;
            }
        }

        Int128 ShortestPathA(string input)
        {
            var dirPad = new DirPad();

            IEnumerable<string> p1 = NumPad.Paths(input);

            p1 = p1.Select(e => dirPad.Path(e, 10));

            return p1.Min(e => e.Size());
        }


        Int128 ShortestPathB(string input)
        {
            var dirPad = new DirPad();

            List<Int128> lengths = [];

            foreach (var ps in NumPad.Paths(input))
            {
                var p = dirPad.Path(ps, 13);
                lengths.Add(dirPad.PathSize(p, 12));
            }

            return lengths.Min();
        }

        public string? CalcA(string[] lines, PuzzleInfo info)
        {
            Int128 sum = 0;

            foreach (var line in lines)
            {
                var pathSize = ShortestPathA(line);

                var num = Convert.ToInt32(line[..^1]);

                sum += pathSize * num;

                Console.WriteLine($"{line}: {pathSize} * {num} = {pathSize * num}");
            }

            return sum.ToString();
        }


        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            Int128 sum = 0;

            foreach (var line in lines)
            {
                var pathSize = ShortestPathB(line);

                var num = Convert.ToInt32(line[..^1]);

                sum += pathSize * num;

                Console.WriteLine($"{line}: {pathSize} * {num} = {pathSize * num}");
            }

            return sum.ToString();
        }
    }
}
