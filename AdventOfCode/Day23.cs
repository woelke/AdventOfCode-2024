
using System.Numerics;
using Utils;

namespace AdventOfCode
{
    public class Day23 : ISolver
    {
        public string PuzzleFolder => nameof(Day23);
        public (string Phase, int Idx)? PuzzleSelector => null;

        Dictionary<string, List<string>> GetLinks(string[] lines)
        {
            Dictionary<string, List<string>> res = [];

            var add = (string k, string v) =>
            {
                if (res.TryGetValue(k, out var x))
                    x.Add(v);
                else
                    res[k] = [v];
            };

            foreach (var (a, b) in lines.Select(e => e.Split('-').To(f => (f[0], f[1]))))
            {
                add(a, b);
                add(b, a);
            }

            return res;
        }

        public string? CalcA(string[] lines, PuzzleInfo info)
        {
            var links = GetLinks(lines);

            var nodes = links.Keys.Order().ToList();

            HashSet<(string A, string B, string C)> res = [];

            foreach (var (a, b) in nodes.Product(nodes).Where(e => e._1 != e._2))
            {
                var la = links[a];
                if (!la.Contains(b))
                    continue;

                var lb = links[b];

                foreach (var x in la.Intersect(lb).Where(e => !(a, b).Contains(e)))
                    res.Add((a, b, x).AsEnumerable().Order().AsTuple3());
            }

            return res
                .Count(e => e.AsEnumerable().Any(f => f.StartsWith('t')))
                .ToString();
        }

        List<string>? FindMaxClique(Dictionary<string, List<string>> links, string node, List<string> nodes, int minNodeCount)
        {
            if (nodes.Size() + 1 < minNodeCount)
                return null;

            List<string> clique = [node, .. nodes];

            foreach (var neigbours in nodes.Select<string, List<string>>(n => [n, .. links[n]]))
                clique = clique.Intersect(neigbours).ToList();

            if (clique.Size() == nodes.Size() + 1)
                return clique;

            if (nodes.Size() < minNodeCount)
                return null;

            foreach (var i in Enumerable.Range(0, nodes.Size()))
            {
                var tmp = FindMaxClique(links, node, [.. nodes[0..(i)], .. nodes[(i + 1)..]], Math.Max(minNodeCount, clique.Size()));
                if (tmp is null)
                    continue;

                if (clique.Size() < tmp.Size())
                    clique = tmp;

                if (clique.Size() == nodes.Size())
                    return clique;
            }

            if (clique.Size() < minNodeCount)
                return null;

            return clique;
        }

        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            var links = GetLinks(lines);

            List<string> res = [];

            foreach (var (node, directNeigbours) in links)
            {
                var tmp = FindMaxClique(links, node, directNeigbours, res.Size() + 1);

                if (tmp is null)
                    continue;

                if (res.Size() < tmp.Size())
                    res = tmp;
            }

            return string.Join(',', res.Order());
        }
    }
}
