using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    internal class Day10 : IDay
    {
        public int Order => 10;

        const string _inputFilename = "day10_input.txt";

        int _register = 1;

        public void Run()
        {
            string[] lines = File.ReadAllLines(_inputFilename);

            int lineIndex = 0;

            bool addOnGoing = false;

            int totalStrength = 0;
            List<char> output = new();

            for (int cycle = 1; cycle <= 240; cycle++)
            {
                var parts = lines[lineIndex].Split(' ');

                switch (cycle)
                {
                    case 20:
                    case 60:
                    case 100:
                    case 140:
                    case 180:
                    case 220:
                        totalStrength += _register * cycle;
                        break;
                }
                int position = (cycle - 1) % 40;
                if (position >= _register - 1 && position <= _register + 1)
                {
                    output.Add('#');
                }
                else
                {
                    output.Add('.');
                }

                if (addOnGoing == true)
                {
                    _register += int.Parse(parts[1]);
                    lineIndex++;
                    addOnGoing = false;
                }
                else
                {
                    if (lineIndex >= lines.Length)
                    {
                        continue;
                    }
                    if (parts[0] == "noop")
                    {
                        lineIndex++;
                    }
                    else if (parts[0] == "addx")
                    {
                        addOnGoing = true;
                    }
                }

            }

            Console.WriteLine($"Total strength: {totalStrength}");

            for(int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    Console.Write(output[(i * 40) + (j)]);
                }
                Console.Write("\n");
            }
        }
    }
}
