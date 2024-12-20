using System.Collections.Frozen;
using System.Text;
using Utils;

namespace AdventOfCode
{
    public class Day14 : ISolver
    {
        public string PuzzleFolder => nameof(Day14);
        public (string Phase, int Idx)? PuzzleSelector => null;

        record struct Pos(int X, int Y);

        IEnumerable<(Pos P, Pos V)> ParseInput(string[] lines)
        {
            foreach (var line in lines)
            {
                yield return line.Split(' ')
                    .To(e =>
                        (e.First()
                            .Split('=')
                            .Second()
                            .Split(',')
                            .To(f =>
                                new Pos(Convert.ToInt32(f.First()), Convert.ToInt32(f.Second()))
                               )
                        , e.Second()
                            .Split('=')
                            .Second()
                            .Split(',')
                            .To(f =>
                                new Pos(Convert.ToInt32(f.First()), Convert.ToInt32(f.Second()))
                               )
                        )
                );
            }
        }

        Pos GetFieldDimensions(PuzzleInfo info)
            => info.Phase switch
            {
                "puzzle" => new(101, 103),
                _ => new(11, 7)
            };

        Pos CalcRobotPos(Pos p, Pos v, int rounds, Pos field)
        {
            var x = (p.X + (v.X * rounds)) % field.X;
            if (x < 0)
                x = field.X + x;

            var y = (p.Y + (v.Y * rounds)) % field.Y;
            if (y < 0)
                y = field.Y + y;

            return new(x, y);
        }

        int GetQuadrant(Pos p, Pos field)
        {
            var xM = field.X / 2;
            var yM = field.Y / 2;

            if (p.X < xM && p.Y < yM)
                return 0;

            if (p.X > xM && p.Y < yM)
                return 1;

            if (p.X < xM && p.Y > yM)
                return 2;

            if (p.X > xM && p.Y > yM)
                return 3;

            return 4;
        }

        public string? CalcA(string[] lines, PuzzleInfo info)
        {
            var field = GetFieldDimensions(info);
            var robs = ParseInput(lines);

            List<int> q = [0, 0, 0, 0, 0];

            foreach (var rob in robs)
            {
                var pos = CalcRobotPos(rob.P, rob.V, 100, field);
                q[GetQuadrant(pos, field)]++;
            }

            return (q[0] * q[1] * q[2] * q[3]).ToString();
        }

        void PrintBathroom(Dictionary<Pos, int> robs, Pos field)
        {
            var res = new StringBuilder();
            foreach (var y in Enumerable.Range(0, field.Y))
            {
                foreach (var x in Enumerable.Range(0, field.X))
                {
                    if (robs.TryGetValue(new Pos(x, y), out var c))
                        res.Append(c);
                    else
                        res.Append('.');
                }

                res.AppendLine();
            }

            Console.WriteLine(res.ToString());
        }

        Dictionary<Pos, int> CalcRobsPoss(List<(Pos P, Pos V)> staringPos, int round, Pos field)
        {
            Dictionary<Pos, int> res = [];

            foreach (var rob in staringPos)
            {
                var pos = CalcRobotPos(rob.P, rob.V, round, field);

                if (res.ContainsKey(pos))
                    res[pos]++;
                else
                    res[pos] = 1;
            }

            return res;
        }

        bool MayContainsTree(Dictionary<Pos, int> robs)
        {
            foreach (var r in robs.Keys)
            {
                int c = 0;
                foreach (var y in Enumerable.Range(0, 5))
                {
                    if (robs.ContainsKey(new(r.X, r.Y + y)))
                        c++;
                    else
                    {
                        c = 0;
                        break;
                    }
                }

                if (c >= 5)
                    return true;
            }

            return false;
        }

        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            var field = GetFieldDimensions(info);
            var robs = ParseInput(lines).ToList();

            foreach (var round in Enumerable.Range(0, 100000))
            {
                Console.WriteLine($"Round: {round}");
                var robsPoss = CalcRobsPoss(robs, round, field);

                if (MayContainsTree(robsPoss))
                {
                    PrintBathroom(robsPoss, field);

                    Console.Write("Press return to contiue ...");
                    Console.ReadLine();
                }
            }

            return null;
        }
    }
}
