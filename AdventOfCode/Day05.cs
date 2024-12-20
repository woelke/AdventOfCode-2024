using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace AdventOfCode
{
    public class Day05 : ISolver
    {
        public string PuzzleFolder => nameof(Day05);
        public (string Phase, int Idx)? PuzzleSelector => null;

        (List<(int A, int B)> Orders, List<List<int>> Updates) ParseInput(string[] lines)
        {
            List<(int A, int B)> orders = [];
            List<List<int>> updates = [];

            foreach (string line in lines)
            {
                if (line.Length == 0)
                    continue;

                if (line.Contains('|'))
                    orders.Add(line.Split('|').To(e => (Convert.ToInt32(e[0]), Convert.ToInt32(e[1]))));
                else
                    updates.Add(line.Split(',').Select(e => Convert.ToInt32(e)).ToList());
            }

            return (orders, updates);
        }



        public string? CalcA(string[] lines, PuzzleInfo info)
        {
            var (orders, updates) = ParseInput(lines);

            List<int> middlesInOrder = [];

            foreach (var update in updates)
            {
                bool notInOrder = false;
                foreach (var (val, idx) in update.WithIndex())
                {
                    if (orders.Where(e => e.B == val && update.Skip(idx).Contains(e.A)).Any())
                    {
                        notInOrder = true;
                        break;
                    }
                }

                if (!notInOrder)
                    middlesInOrder.Add(update[update.Count / 2]);
            }

            return middlesInOrder.Sum().ToString();
        }

        public List<int> OrderMe(List<(int A, int B)> orders, List<int> xs)
        {
            List<int> ys = [.. xs];
            foreach (var i in Enumerable.Range(0, ys.Count - 1))
            {
                if (orders.Contains((ys[i + 1], ys[i])))
                {
                    var t = (ys[i], ys[i + 1]);
                    ys[i] = t.Item2;
                    ys[i + 1] = t.Item1;
                    return OrderMe(orders, ys);
                }
            }

            return ys;
        }


        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            var (orders, updates) = ParseInput(lines);

            List<List<int>> unordereds = [];

            foreach (var update in updates)
            {
                foreach (var (val, idx) in update.WithIndex())
                {
                    if (orders.Where(e => e.B == val && update.Skip(idx).Contains(e.A)).Any())
                    {
                        unordereds.Add(update);
                        break;
                    }
                }
            }

            return unordereds.Select(e => OrderMe(orders, e)).Select(e => e[e.Count / 2]).Sum().ToString();
        }
    }
}
