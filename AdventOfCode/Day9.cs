using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace AdventOfCode
{
    public class Day9 : ISolver
    {
        public int Day => 9;

        public List<int?> ParseInput(string[] lines)
        {
            int id = 0;
            bool isId = true;
            List<int?> disk = [];

            foreach (var count in lines[0].Select(e => Convert.ToInt32(e.ToString())))
            {
                if (isId)
                {
                    disk.AddRange(Enumerable.Repeat<int?>(id, count));
                    id++;
                }
                else
                    disk.AddRange(Enumerable.Repeat<int?>(null, count));

                isId = !isId;
            }

            return disk;
        }

        public string? CalcA(string[] lines)
        {
            var disk = ParseInput(lines);

            int lId = 0;
            int rId = 0;

            var rdisk = disk.ToList();
            rdisk.Reverse();
            while (true)
            {
                lId = disk.FindIndex(lId, e => e is null);
                rId = rdisk.FindIndex(rId, e => e is not null);

                if (lId >= (disk.Count - (rId + 1)))
                    break;

                disk.Swap(lId, disk.Count - (rId + 1));
                rdisk.Swap(rId, rdisk.Count - (lId + 1));
            }

            return disk.Where(e => e.HasValue).WithIndex().Select(e => (Int128)(e.Item * e.Idx)).Sum().ToString();
        }

        void PrintDisk(List<int?> disk)
        {
            foreach (var i in disk)
                Console.Write($"{(i is null ? "." : i.ToString())}");

            Console.WriteLine();
        }

        public string? CalcB(string[] lines)
        {
            var disk = ParseInput(lines);

            List<(int BlockId, int StartIdx, int Count)> fileIndex = [];

            int blockId = 0;
            int startIdx = 0;
            int count = 0;

            foreach (var (id, idx) in disk.WithIndex().Where(e => e.Item.HasValue))
            {
                if (blockId == id)
                    count++;
                else
                {
                    fileIndex.Add((blockId, startIdx, count));
                    blockId = id!.Value;
                    startIdx = idx;
                    count = 1;
                }
            }
            fileIndex.Add((blockId, startIdx, count));
            fileIndex.Reverse();

            while (fileIndex.Count != 0)
            {
                var block = fileIndex[0];
                fileIndex.RemoveAt(0);

                int? freeSpace = FindFreeSpace(disk, block.Count, block.StartIdx);

                if (!freeSpace.HasValue)
                    continue;

                foreach (var i in Enumerable.Range(0, block.Count))
                    disk.Swap(freeSpace.Value + i, block.StartIdx + i);
            }

            return disk.WithIndex().Where(e => e.Item.HasValue).Select(e => (Int128)(e.Item * e.Idx)).Sum().ToString();
        }

        public int? FindFreeSpace(List<int?> disk, int givenCount, int maxIdx)
        {
            int? startIdx = null;
            int count = 0;

            foreach (var (id, idx) in disk.WithIndex())
            {
                if (idx > maxIdx)
                    return null;

                if (id.HasValue)
                {
                    startIdx = null;
                    count = 0;
                }
                else
                {
                    if (startIdx == null)
                        startIdx = idx;

                    count++;

                    if (count >= givenCount)
                        return startIdx;
                }
            }

            if (count >= givenCount)
                return startIdx;
            else
                return null;
        }
    }
}
