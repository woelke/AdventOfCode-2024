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

        static readonly List<List<char>> NumPad = [
            [.. "789"],
            [.. "456"],
            [.. "123"],
            [.. "x0A"],
            ];

        static readonly List<List<char>> DirPad = [
            [.. "x^A"],
            [.. "<v>"],
            ];

        record Pad(List<List<char>> Bs)
        {

            char Get(Pos p) => Bs[p.Y][p.X];

            Pos ToPos(char b)
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

            private IEnumerable<List<char>> Paths(char a, char b)
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
                        yield return l.Skip(1).Select(e => e.D).Append('A').ToList();

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

            public IEnumerable<List<char>> Paths(IEnumerable<char> input)
            {
                IEnumerable<IEnumerable<char>> ls = Paths(input.First(), input.Second());
                foreach (var (a, b) in input.Skip(1).Zip(input.Skip(2)))
                    ls = ls.Product(Paths(a, b)).Select(e => e._1.Concat(e._2));

                foreach (var l in ls)
                    yield return l.ToList();
            }
        }

        List<char> ShortestPath(List<char> input)
        {
            var numPad = new Pad(NumPad);
            var dirPad = new Pad(DirPad);

            IEnumerable<IEnumerable<char>> p1 = numPad.Paths(input.Prepend('A'));
            IEnumerable<IEnumerable<char>> p2 = p1.Select(e => dirPad.Paths(e.Prepend('A'))).SelectMany(e => e);
            IEnumerable<IEnumerable<char>> p3 = p2.Select(e => dirPad.Paths(e.Prepend('A'))).SelectMany(e => e);

            return p3.MinBy(e => e.Size())!.ToList();
        }

        public string? CalcA(string[] lines, PuzzleInfo info)
        {
            var sum = 0;

            foreach (var line in lines)
            {

                var path = ShortestPath(line.ToList());

                var num = Convert.ToInt32(line[..^1]);

                sum += path.Size() * num;

                Console.WriteLine($"{line}: {path.Size()} * {num} = {path.Size() * num}");
            }

            return sum.ToString();
        }

        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            return null;
        }
    }
}
