using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    public class Day01 : IDay
    {
        public int Order => 1;

        const string _inputFilename = "day01_input.txt";

        public void Run()
        {
            string[] lines = File.ReadAllLines(_inputFilename);

            List<int> elfCalories = new();
            int i = 0;
            bool newEntry = true;
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    i++;
                    newEntry = true;
                    continue;
                }
                int calories = int.Parse(line);
                if(newEntry)
                {
                    elfCalories.Add(calories);
                    newEntry = false;
                }
                else
                {
                    elfCalories[i] += calories;
                }
            }

            Console.WriteLine($"Max calories: {elfCalories.Max()}");

            var topThreeTotal = elfCalories.OrderByDescending(x => x)
                                           .Take(3)
                                           .Sum();
            Console.WriteLine($"Top three total calories: {topThreeTotal}");
        }
    }
}
