using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    internal class Day04 : IDay
    {
        public int Order => 4;
        const string _inputFilename = "day04_input.txt";

        public void Run()
        {
            string[] lines = File.ReadAllLines(_inputFilename);

            int overlapsFully = 0;
            int overlapsPArtially = 0;
            foreach (string line in lines)
            {
                string firstElfInput = line.Split(',').First();
                string secondElfInput = line.Split(',').Skip(1).First();

                (int start, int end) firstElf =
                    new(int.Parse(firstElfInput.Split('-').First()),
                        int.Parse(firstElfInput.Split('-').Skip(1).First()));

                (int start, int end) secondtElf =
                    new(int.Parse(secondElfInput.Split('-').First()),
                        int.Parse(secondElfInput.Split('-').Skip(1).First()));

                if (ElfsOverlapFully(firstElf, secondtElf))
                {
                    overlapsFully++;
                }
                if (ElfsOverlapPartially(firstElf, secondtElf))
                {
                    overlapsPArtially++;
                }
            }

            Console.WriteLine($"Overlaps fully: {overlapsFully}");
            Console.WriteLine($"Overlaps partially: {overlapsPArtially}");
        }

        public bool ElfsOverlapFully((int start, int end) first, (int start, int end) second)
        {
            if (first.start >= second.start && first.end <= second.end)
            {
                return true;
            }
            if (second.start >= first.start && second.end <= first.end)
            {
                return true;
            }
            return false;
        }

        public bool ElfsOverlapPartially((int start, int end) first, (int start, int end) second)
        {
            if (first.start >= second.start && first.start <= second.end)
            {
                return true;
            }
            if (first.end >= second.start && first.end <= second.end)
            {
                return true;
            }
            if (second.start >= first.start && second.start <= first.end)
            {
                return true;
            }
            if (second.end >= first.start && second.end <= first.end)
            {
                return true;
            }
            return false;
        }
    }
}
