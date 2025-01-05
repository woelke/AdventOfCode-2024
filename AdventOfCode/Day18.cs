using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using Utils;

namespace AdventOfCode
{
    public class Day18 : ISolver
    {
        public string PuzzleFolder => nameof(Day18);
        public (string Phase, int Idx)? PuzzleSelector => null;

        readonly record struct Pos(int X, int Y)
        {
            public Pos Left() => new(X - 1, Y);
            public Pos Top() => new(X, Y - 1);
            public Pos Right() => new(X + 1, Y);
            public Pos Bottom() => new(X, Y + 1);
        }

        class Prop()
        {
            public int Cost = int.MaxValue;
        }

        Dictionary<Pos, Prop> GetMem(string[] lines, int count, int dim)
        {
            HashSet<Pos> corrupted = [];
            Dictionary<Pos, Prop> res = [];

            foreach (var line in lines.Take(count))
            {
                var pos = line.Split(',').To(v => new Pos(Convert.ToInt32(v[0]), Convert.ToInt32(v[1])));
                corrupted.Add(pos);
            }

            foreach (var pos in Enumerable.Range(0, dim + 1).Product(Enumerable.Range(0, dim + 1)).Select(e => new Pos(e._1, e._2)))
            {
                if (!corrupted.Contains(pos))
                    res[pos] = new Prop();
            }

            return res;
        }

        void Print(Dictionary<Pos, Prop> mem, int dim)
        {
            var res = new StringBuilder();


            foreach (var y in Enumerable.Range(0, dim + 1))
            {

                foreach (var x in Enumerable.Range(0, dim + 1))
                {
                    if (!mem.TryGetValue(new Pos(x, y), out var prop))
                        res.Append('#');
                    else
                    {
                        res.Append(prop.Cost switch
                        {
                            < 10 => '.',
                            int.MaxValue => "x",
                            _ => '?'
                        });
                    }
                }

                res.AppendLine();
            }

            Console.WriteLine(res.ToString());
        }

        void FillInCost(Dictionary<Pos, Prop> mem, Pos pos, int cost)
        {
            if (!mem.TryGetValue(pos, out var prop))
                return;

            if (prop.Cost <= cost)
                return;

            prop.Cost = cost;

            FillInCost(mem, pos.Bottom(), cost + 1);
            FillInCost(mem, pos.Right(), cost + 1);
            FillInCost(mem, pos.Left(), cost + 1);
            FillInCost(mem, pos.Top(), cost + 1);
        }

        public string? CalcA(string[] lines, PuzzleInfo info)
        {
            var (memCount, dim) = info.Phase switch
            {
                "puzzle" => (1024, 70),
                _ => (12, 6)
            };

            var mem = GetMem(lines, memCount, dim);

            FillInCost(mem, new Pos(0, 0), 0);

            return mem[new Pos(dim, dim)].Cost.ToString();
        }

        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            var (minMemCount, dim) = info.Phase switch
            {
                "puzzle" => (1024, 70),
                _ => (12, 6)
            };

            foreach (var memCount in Enumerable.Range(minMemCount, lines.Size() - minMemCount))
            {
                var mem = GetMem(lines, memCount, dim);

                FillInCost(mem, new Pos(0, 0), 0);

                Console.WriteLine($"Test ({memCount}): {lines[memCount]}");

                if (mem[new Pos(dim, dim)].Cost == int.MaxValue)
                    return lines[memCount-1];
            }

            return null;
        }
    }
}
