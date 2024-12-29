using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Utils;

namespace AdventOfCode
{
    public class Day16 : ISolver
    {
        public string PuzzleFolder => nameof(Day16);
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

        (Dictionary<Pos, Dictionary<char, int>> M, Pos S, Pos E) ParseInput(string[] lines)
        {
            Dictionary<Pos, Dictionary<char, int>> m = [];
            var s = new Pos(0, 0);
            var e = new Pos(0, 0);

            foreach (var (line, y) in lines.WithIndex())
            {
                foreach (var (c, x) in line.WithIndex())
                {
                    var p = new Pos(x, y);
                    if (c == 'S')
                        s = p;
                    else if (c == 'E')
                        e = p;

                    if (('.', 'S', 'E').Contains(c))
                        m[p] = new() { { '>', int.MaxValue }, { 'v', int.MaxValue }, { '<', int.MaxValue }, { '^', int.MaxValue } };
                }
            }

            m[s]['>'] = 0;
            m[s]['^'] = 1000;
            m[s]['v'] = 1000;
            m[s]['<'] = 1000;
            return (m, s, e);
        }

        static Dictionary<char, char> _ndl = new() {
            {'>', 'v'},
            {'v', '<'},
            {'<', '^'},
            {'^', '>'},
        };

        static Dictionary<char, char> _odl = new() {
            {'>', '<'},
            {'v', '^'},
            {'<', '>'},
            {'^', 'v'},
        };

        IEnumerable<(Pos Np, char Nd, int Nc)> Next(Pos p, char d, int c)
        {
            yield return (p.Next(d), d, c + 1);

            var nd = _ndl[d];
            yield return (p.Next(nd), nd, c + 1001);

            nd = _ndl[nd];
            yield return (p.Next(nd), nd, c + 2001);

            nd = _ndl[nd];
            yield return (p.Next(nd), nd, c + 1001);
        }

        void SolveMaze(Dictionary<Pos, Dictionary<char, int>> m, Pos p, char d)
        {
            var cs = m[p];

            foreach (var (np, nd, nc) in Next(p, d, cs[d]))
            {
                if (!m.TryGetValue(np, out var ancs))
                    continue;

                if (ancs[nd] <= nc)
                    continue;

                m[np][nd] = nc;
                SolveMaze(m, np, nd);
            }
        }

        public string? CalcA(string[] lines, PuzzleInfo info)
        {
            var (m, s, e) = ParseInput(lines);
            SolveMaze(m, s, '>');
            return m[e].Select(e => e.Value).Min().ToString();
        }

        HashSet<Pos> GetTiles(Dictionary<Pos, Dictionary<char, int>> m, Pos p, int lc, Pos s)
        {
            if (p == s)
                return [];

            HashSet<Pos> res = [];

            foreach (var (d, c) in m[p])
            {
                if (!(1, 1001).Contains(lc - c))
                    continue;

                var pd = _odl[d];
                var pp = p.Next(pd);

                if (!m.TryGetValue(pp, out var pc))
                    continue;

                res.Add(pp);

                res.UnionWith(GetTiles(m, pp, c, s));
            }

            return res;
        }

        void PrintMaze(Dictionary<Pos, int> m, HashSet<Pos> tiles, Pos s, Pos e)
        {
            var maxX = m.Keys.Select(e => e.X).Max();
            var maxY = m.Keys.Select(e => e.Y).Max();

            var res = new StringBuilder();

            foreach (var y in Enumerable.Range(0, maxY + 2))
            {
                foreach (var x in Enumerable.Range(0, maxX + 2))
                {
                    var p = new Pos(x, y);
                    if (tiles.Contains(p))
                        res.Append('O');
                    else if (m.ContainsKey(p))
                        res.Append('.');
                    else
                        res.Append('#');
                }

                res.AppendLine();
            }

            res.AppendLine();

            Console.WriteLine(res.ToString());
        }

        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            var (m, s, e) = ParseInput(lines);
            SolveMaze(m, s, '>');
            //PrintMaze(m, [], s, e);

            var res = GetTiles(m, e, m[e].Select(e => e.Value).Min() + 1, s);
            res.Add(e);

            //PrintMaze(m, res, s, e);


            //m.PrintMe();
            return res.Size().ToString();
        }
    }
}
