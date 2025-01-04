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
    public class Day17 : ISolver
    {
        public string PuzzleFolder => nameof(Day17);
        public (string Phase, int Idx)? PuzzleSelector => null;

        class Computer()
        {
            public int InstCounter = 0;
            public BigInteger RegA;
            public BigInteger RegB;
            public BigInteger RegC;
            public List<int> Program = [];
            public List<int> Out = [];

            static public Computer FromInput(string[] input)
            {
                var a = Convert.ToInt32(input[0].Split(": ")[1]);
                var b = Convert.ToInt32(input[1].Split(": ")[1]);
                var c = Convert.ToInt32(input[2].Split(": ")[1]);

                var nums = input[4].Split(": ")[1].Split(',').Select(e => Convert.ToInt32(e)).ToList();

                return new Computer()
                {
                    RegA = a,
                    RegB = b,
                    RegC = c,
                    Program = nums,
                };
            }

            public Computer DeepCopy()
                => new()
                {
                    RegA = RegA,
                    RegB = RegB,
                    RegC = RegB,
                    Program = Program.ToList(),
                };

            public bool Next()
            {
                if (InstCounter + 1 >= Program.Size())
                    return false;

                var opcode = Program[InstCounter];
                var operand = Program[InstCounter + 1];

                switch (opcode)
                {
                    case 0: // adv
                        {
                            var dis = BigInteger.Pow(2, GetComboOperand(operand).ToInt());
                            if (dis == 0)
                                return false;

                            RegA = RegA / dis;
                            InstCounter += 2;
                        }
                        break;
                    case 1: // bxl
                        RegB = RegB ^ operand;
                        InstCounter += 2;
                        break;
                    case 2: // bst
                        RegB = GetComboOperand(operand) % 8;
                        InstCounter += 2;
                        break;
                    case 3: // jnz
                        if (RegA != 0)
                            InstCounter = operand;
                        else
                            InstCounter += 2;
                        break;
                    case 4: // bxc
                        RegB = RegB ^ RegC;
                        InstCounter += 2;
                        break;
                    case 5: // out
                        Out.Add(GetComboOperand(operand).ToInt() % 8);
                        InstCounter += 2;
                        break;
                    case 6: // bdv
                        {
                            var dis = BigInteger.Pow(2, GetComboOperand(operand).ToInt());
                            if (dis == 0)
                                return false;
                            RegB = RegA / dis;
                            InstCounter += 2;
                        }
                        break;
                    case 7: // cdv
                        {
                            var dis = BigInteger.Pow(2, GetComboOperand(operand).ToInt());
                            if (dis == 0)
                                return false;
                            RegC = RegA / dis;
                            InstCounter += 2;
                        }
                        break;
                    case 9: // out
                        Out.Add(GetComboOperand(operand).ToInt());
                        InstCounter += 2;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                return true;
            }


            public bool NextImproved()
            {
                var a = RegA >> ((RegA & 0b_111).ToInt() ^ 0b_010);
                var x = (RegA ^ a ^ 0b_101) & 0b_111;
                Out.Add(x.ToInt());

                RegA = RegA >> 3;

                return RegA != 0;
            }


            private BigInteger GetComboOperand(int operand)
                => operand switch
                {
                    <= 3 => operand,
                    4 => RegA,
                    5 => RegB,
                    6 => RegC,
                    _ => throw new NotImplementedException()
                };
        }

        public string? CalcA(string[] lines, PuzzleInfo info)
        {
            var c = Computer.FromInput(lines);
            while (c.NextImproved()) ;

            return string.Join(",", c.Out);
        }

        BigInteger? CalcRegA(List<int> program, BigInteger A)
        {
            if (program.IsEmpty())
                return A;

            foreach (var a in Enumerable.Range(0, 8))
            {
                BigInteger regA = (A << 3) | a;

                var v = regA >> ((regA & 0b_111).ToInt() ^ 0b_010);
                var w = (regA ^ v ^ 0b_101) & 0b_111;

                if (w == program[^1])
                {
                    var res = CalcRegA(program[..^1], regA);
                    if (res.HasValue)
                        return res.Value;
                }
            }

            return null;
        }


        public string? CalcB(string[] lines, PuzzleInfo info)
        {
            var c = Computer.FromInput(lines);

            var res = CalcRegA(c.Program, 0);

            if (res.HasValue)
            {
                var testC = c.DeepCopy();
                testC.RegA = res.Value;
                while (testC.NextImproved()) ;
                Console.WriteLine(string.Join(",", testC.Out));
            }

            return res.ToString();
        }
    }
}
