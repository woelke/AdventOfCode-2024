using Utils;

namespace AdventOfCode
{
    public class PuzzleSetup(ISolver solver)
    {
        private readonly string InputPath = $"C:\\Users\\woelke\\workspace\\AdventOfCode-2024\\AdventOfCode\\Input\\Day{solver.Day}";

        private record Input(string Phase, int Idx, Mut<string[]?> PuzzleInput, Mut<string?> Result);

        private List<Input> _exampleInputs = [];
        private string[] _puzzleInput = null!;

        private void LoadInput()
        {
            foreach (var file in Directory.GetFiles(InputPath))
            {
                var split = Path.GetFileName(file).Split("_");
                if (split.Length == 1)
                {
                    _puzzleInput = File.ReadAllLines(file);
                }
                else if (split.Length == 3)
                {
                    var phase = split[0];
                    var idx = Convert.ToInt32(split[1]);
                    bool isResult = split[2].StartsWith("result");

                    var input = _exampleInputs.Find(e => e.Phase == phase && e.Idx == idx);
                    if (input is null)
                    {
                        if (isResult)
                            _exampleInputs.Add(new(phase, idx, new(null), new(File.ReadAllText(file))));
                        else
                            _exampleInputs.Add(new(phase, idx, new(File.ReadAllLines(file)), new(null)));
                    }
                    else
                    {
                        if (isResult)
                            input.Result.Val = File.ReadAllText(file);
                        else
                            input.PuzzleInput.Val = File.ReadAllLines(file);
                    }
                }
            }

            _exampleInputs = [.. _exampleInputs.OrderBy(e => (e.Phase, e.Idx))];
        }

        public void CalcAll()
        {
            LoadInput();

            foreach (var input in _exampleInputs)
            {
                PrintExample(input);
            }

            Console.WriteLine();

            PrintPuzzle();
        }

        private void PrintExample(Input input)
        {
            string? res;

            if (input.Phase == "a")
                res = solver.CalcA(input.PuzzleInput.Val!)?.Trim();
            else if (input.Phase == "b")
                res = solver.CalcB(input.PuzzleInput.Val!)?.Trim();
            else
                throw new ArgumentException("Phase neither a nor b");

            Console.WriteLine($"{input.Phase}_{input.Idx} Result: \"{res}\" ({(res == input.Result.Val!.Trim() ? "Ok" : "Failed")})");
        }

        private void PrintPuzzle()
        {
            Console.WriteLine($" Result A: {solver.CalcA(_puzzleInput)}");
            Console.WriteLine($" Result B: {solver.CalcB(_puzzleInput)}");
        }
    }
}
