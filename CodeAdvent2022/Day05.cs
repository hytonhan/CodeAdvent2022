using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    internal class Day05 : IDay
    {
        public int Order => 5;

        const string _inputFilename = "day05_input.txt";
        public void Run()
        {
            string[] lines = File.ReadAllLines(_inputFilename);


            List<Stack<char>> stacks = new();
            List<Stack<char>> stacks2 = new();

            for (int i = 0; i < 9; i++)
            {
                stacks.Add(new Stack<char>());
                stacks2.Add(new Stack<char>());
            }
            for (int i = 0; i < 8; i++)
            {
                var line = lines[i];
                int start = 0;
                while (line.IndexOf('[', start) != -1)
                {
                    start = line.IndexOf('[', start) + 1;
                    var @char = line[start];
                    stacks[(start - 1) / 4].Push(@char);
                    stacks2[(start - 1) / 4].Push(@char);
                }
            }
            for (int i = 0; i < 9; i++)
            {
                stacks[i] = new Stack<char>(stacks[i]);
                stacks2[i] = new Stack<char>(stacks2[i]);
            }

            for (int i = 10; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(' ');
                int count = int.Parse(parts[1]);
                int from = int.Parse(parts[3]) - 1;
                int to = int.Parse(parts[5]) - 1;

                //Stacks 1 by 1
                for (int j = 0; j < count; j++)
                {
                    char temp = stacks[from].Pop();
                    stacks[to].Push(temp);
                }
                //Stacks all ot once
                Stack<char> temp2 = new();
                for (int j = 0; j < count; j++)
                {
                    temp2.Push(stacks2[from].Pop());
                }
                for (int j = 0; j < count; j++)
                {
                    stacks2[to].Push(temp2.Pop());
                }
            }

            Console.Write($"Top boxes 1-by-1: ");
            for (int i = 0; i < 9; i++)
            {
                Console.Write($"{stacks[i].Pop()}");
            }
            Console.WriteLine(".");
            Console.Write($"Top boxes all at once: ");
            for (int i = 0; i < 9; i++)
            {
                Console.Write($"{stacks2[i].Pop()}");
            }
            Console.WriteLine(".");
        }
    }
}
