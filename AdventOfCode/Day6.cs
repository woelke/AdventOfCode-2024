using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace AdventOfCode
{
    public class Day6 : ISolver
    {
        public int Day => 6;

        public string? CalcA(string[] lines)
        {
            List<List<char>> map = lines.Select(e => e.ToCharArray().ToList()).ToList(); // y, x

            var (gc, gy, gx) = map.WithIndex()
                .Select(l => l.Item.WithIndex().Select(z => (z.Item, l.Idx, z.Idx)))
                .SelectMany(e => e)
                .Where(e => e.Item != '.' && e.Item != '#')
                .First();

            var (ngc, ngy, ngx) = (gc, gy, gx);

            while (true)
            {
                if (gc == '^')
                    ngy--;
                else if (gc == '>')
                    ngx++;
                else if (gc == 'v')
                    ngy++;
                else // <
                    ngx--;

                if (ngy < 0 || ngy >= map.Count || ngx < 0 || ngx >= map[0].Count)
                {
                    map[gy][gx] = 'X';
                    break;
                }

                ngc = map[ngy][ngx];

                if (ngc == '.' || ngc == 'X')
                {
                    map[ngy][ngx] = gc;
                    map[gy][gx] = 'X';
                    (gy, gx) = (ngy, ngx);
                }
                else // #
                {
                    gc = gc switch
                    {
                        '^' => '>',
                        '>' => 'v',
                        'v' => '<',
                        _ => '^'
                    };
                    (ngy, ngx) = (gy, gx);
                }
            }

            return map.Select(l => l.Select(e => e == 'X' ? 1 : 0).Sum()).Sum().ToString();
        }

        private bool LeftMap(List<List<(char, int)>> map)
        {
            var (gc, gy, gx) = map.WithIndex()
                .Select(l => l.Item.WithIndex().Select(z => (z.Item.Item1, l.Idx, z.Idx)))
                .SelectMany(e => e)
                .Where(e => "^>v<".Contains(e.Item1))
                .First();

            var (ngy, ngx) = (gy, gx);
            int from;

            while (true)
            {
                if (gc == '^')
                {
                    ngy--;
                    from = 1;
                }
                else if (gc == '>')
                {
                    ngx++;
                    from = 2;
                }
                else if (gc == 'v')
                {
                    ngy++;
                    from = 4;
                }
                else // <
                {
                    ngx--;
                    from = 8;
                }

                if (ngy < 0 || ngy >= map.Count || ngx < 0 || ngx >= map[0].Count)
                    return true;

                var (ngc, nfrom) = map[ngy][ngx];

                if (ngc == '.' && (nfrom & from) != 0)
                    return false;
                else if (ngc == '.')
                {
                    map[ngy][ngx] = (gc, nfrom | from);
                    map[gy][gx] = ('.', map[gy][gx].Item2);
                    (gy, gx) = (ngy, ngx);
                }
                else if (ngc == '#')
                {
                    gc = gc switch
                    {
                        '^' => '>',
                        '>' => 'v',
                        'v' => '<',
                        _ => '^'
                    };
                    (ngy, ngx) = (gy, gx);
                }
            }
        }

        public string? CalcB(string[] lines)
        {
            var refMap = lines.Select(e => e.ToCharArray().Select(f => (f, 0)).ToList()).ToList(); // y, x

            var res = 0;

            foreach (var (y, x) in Enumerable.Range(0, refMap.Count).Product(Enumerable.Range(0, refMap[0].Count)))
            {
                if (refMap[y][x].Item1 != '.')
                    continue;

                var map = refMap.Select(l => l.ToList()).ToList();
                map[y][x] = ('#', 0);

                if (!LeftMap(map))
                    res++;
            }

            return res.ToString();
        }
    }
}
