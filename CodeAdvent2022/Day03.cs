using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    internal class Day03 : IDay
    {
        public int Order => 3;

        const string _inputFilename = "day03_input.txt";

        const int letterCount = (int)('z' - 'a');
        public void Run()
        {
            char foo = 'a';
            char bar = 'A';

            Console.WriteLine($"a: {(int)foo}. A: {(int)bar}. a+25: {(char)(foo + 25)}. A + 25: {(char)(bar + 25)}");

            int sum = 0;
            string[] lines = File.ReadAllLines(_inputFilename);
            int i = 0;
            int elfGroups = 0;
            string elfGroup = "";
            foreach (string line in lines)
            {
                i++;
                if (i == 1)
                {
                    elfGroup = line;
                }
                if (i == 2)
                {
                    elfGroup = new string(elfGroup.Intersect(line).ToArray());
                }
                if (i == 3)
                {
                    i = 0;
                    elfGroups += CharToPriorityConverter(elfGroup.Intersect(line).Single());
                }
                var ruckSackLength = line.Length;

                var firstCompartment = line.Substring(0, ruckSackLength / 2);
                var secondCompartment = line.Substring(ruckSackLength / 2);

                var common = firstCompartment.Intersect(secondCompartment).Single();
                sum += CharToPriorityConverter(common);
            }

            Console.WriteLine($"Sum of priorities: {sum}");
            Console.WriteLine($"Sum of elfgroups: {elfGroups}");
        }

        int CharToPriorityConverter(char character)
        {
            if ((int)character - 'a' >= 0 && (int)character - 'a' <= letterCount)
            {
                return (int)character - 'a' + 1;
            }
            if ((int)character - 'A' >= 0 && (int)character - 'A' <= letterCount)
            {
                return (int)character - 'A' + 2 + letterCount;
            }
            return -1;
        }
    }
}
