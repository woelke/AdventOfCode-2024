using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public interface ISolver
    {
        string PuzzleFolder { get; }
        (string Phase, int Idx)? PuzzleSelector { get; }

        string? CalcA(string[] lines);
        string? CalcB(string[] lines);
    }
}
