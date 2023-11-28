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

        public void Run()
        {
            string[] lines = _input.Split(Environment.NewLine);


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

        private const string _input = @"[P]     [C]         [M]            
[D]     [P] [B]     [V] [S]        
[Q] [V] [R] [V]     [G] [B]        
[R] [W] [G] [J]     [T] [M]     [V]
[V] [Q] [Q] [F] [C] [N] [V]     [W]
[B] [Z] [Z] [H] [L] [P] [L] [J] [N]
[H] [D] [L] [D] [W] [R] [R] [P] [C]
[F] [L] [H] [R] [Z] [J] [J] [D] [D]
 1   2   3   4   5   6   7   8   9 

move 4 from 9 to 1
move 6 from 3 to 1
move 7 from 4 to 1
move 2 from 8 to 5
move 1 from 9 to 7
move 1 from 8 to 5
move 3 from 6 to 4
move 6 from 1 to 5
move 14 from 1 to 2
move 1 from 6 to 1
move 2 from 6 to 2
move 9 from 5 to 9
move 2 from 4 to 5
move 2 from 5 to 3
move 6 from 9 to 6
move 4 from 1 to 2
move 2 from 1 to 2
move 5 from 6 to 1
move 1 from 4 to 9
move 4 from 9 to 4
move 2 from 3 to 7
move 2 from 4 to 9
move 2 from 9 to 6
move 5 from 2 to 9
move 1 from 4 to 9
move 1 from 4 to 3
move 5 from 9 to 8
move 1 from 6 to 5
move 3 from 7 to 5
move 2 from 1 to 6
move 5 from 6 to 8
move 1 from 9 to 4
move 1 from 6 to 5
move 9 from 2 to 7
move 1 from 2 to 3
move 1 from 4 to 6
move 8 from 5 to 4
move 1 from 6 to 1
move 2 from 8 to 6
move 1 from 6 to 4
move 7 from 4 to 6
move 1 from 3 to 1
move 1 from 3 to 4
move 3 from 4 to 1
move 2 from 3 to 4
move 2 from 4 to 5
move 3 from 5 to 7
move 7 from 8 to 2
move 5 from 1 to 2
move 12 from 7 to 6
move 2 from 1 to 9
move 2 from 9 to 1
move 1 from 7 to 5
move 6 from 2 to 3
move 5 from 2 to 6
move 6 from 2 to 6
move 4 from 3 to 1
move 3 from 2 to 1
move 1 from 5 to 4
move 7 from 1 to 2
move 1 from 4 to 8
move 7 from 2 to 9
move 5 from 2 to 8
move 2 from 6 to 8
move 21 from 6 to 9
move 8 from 9 to 1
move 2 from 6 to 1
move 3 from 8 to 7
move 6 from 6 to 4
move 7 from 1 to 8
move 1 from 9 to 1
move 7 from 7 to 3
move 1 from 7 to 4
move 1 from 7 to 4
move 7 from 8 to 1
move 5 from 4 to 8
move 10 from 1 to 2
move 3 from 1 to 4
move 3 from 2 to 9
move 1 from 4 to 5
move 3 from 3 to 6
move 1 from 6 to 4
move 1 from 6 to 7
move 1 from 7 to 8
move 7 from 2 to 4
move 10 from 9 to 1
move 10 from 4 to 5
move 2 from 5 to 2
move 2 from 2 to 1
move 11 from 8 to 9
move 7 from 1 to 4
move 1 from 6 to 1
move 1 from 8 to 3
move 1 from 4 to 6
move 6 from 4 to 5
move 1 from 5 to 7
move 1 from 6 to 8
move 6 from 1 to 6
move 19 from 9 to 2
move 1 from 1 to 8
move 1 from 4 to 7
move 9 from 2 to 6
move 1 from 9 to 2
move 2 from 8 to 1
move 1 from 1 to 9
move 7 from 3 to 6
move 3 from 9 to 2
move 5 from 2 to 6
move 1 from 9 to 3
move 15 from 6 to 7
move 6 from 6 to 7
move 1 from 1 to 9
move 5 from 6 to 2
move 1 from 6 to 1
move 6 from 5 to 8
move 1 from 3 to 4
move 1 from 9 to 7
move 6 from 8 to 1
move 3 from 4 to 6
move 1 from 6 to 1
move 3 from 5 to 2
move 1 from 5 to 7
move 5 from 1 to 5
move 2 from 6 to 9
move 2 from 9 to 2
move 7 from 5 to 1
move 1 from 5 to 7
move 1 from 5 to 9
move 20 from 7 to 1
move 23 from 1 to 7
move 1 from 1 to 2
move 4 from 7 to 9
move 4 from 9 to 8
move 1 from 9 to 2
move 16 from 7 to 6
move 4 from 1 to 5
move 9 from 7 to 6
move 11 from 2 to 6
move 1 from 1 to 9
move 1 from 1 to 7
move 1 from 8 to 2
move 1 from 9 to 7
move 4 from 5 to 2
move 3 from 8 to 3
move 2 from 2 to 4
move 2 from 7 to 4
move 4 from 4 to 9
move 28 from 6 to 9
move 5 from 2 to 7
move 8 from 6 to 5
move 6 from 2 to 6
move 2 from 7 to 3
move 5 from 5 to 7
move 1 from 5 to 9
move 14 from 9 to 4
move 18 from 9 to 8
move 5 from 6 to 4
move 6 from 7 to 8
move 1 from 2 to 6
move 19 from 4 to 7
move 1 from 2 to 5
move 1 from 9 to 3
move 2 from 5 to 2
move 14 from 7 to 3
move 1 from 5 to 3
move 12 from 8 to 6
move 6 from 6 to 5
move 4 from 5 to 4
move 21 from 3 to 4
move 10 from 8 to 3
move 2 from 3 to 2
move 7 from 4 to 6
move 2 from 8 to 1
move 2 from 2 to 3
move 5 from 7 to 2
move 2 from 1 to 4
move 3 from 3 to 7
move 2 from 5 to 7
move 2 from 2 to 7
move 2 from 2 to 3
move 7 from 4 to 1
move 3 from 1 to 4
move 3 from 2 to 5
move 2 from 1 to 5
move 7 from 4 to 3
move 15 from 6 to 2
move 1 from 1 to 4
move 1 from 5 to 1
move 14 from 3 to 1
move 9 from 4 to 1
move 5 from 7 to 1
move 1 from 3 to 5
move 1 from 4 to 2
move 20 from 1 to 2
move 17 from 2 to 5
move 1 from 3 to 7
move 5 from 7 to 3
move 6 from 5 to 1
move 3 from 3 to 2
move 10 from 1 to 9
move 3 from 5 to 6
move 12 from 5 to 6
move 1 from 5 to 1
move 15 from 6 to 5
move 13 from 5 to 3
move 1 from 5 to 1
move 10 from 3 to 2
move 3 from 3 to 2
move 1 from 5 to 3
move 2 from 3 to 6
move 1 from 3 to 4
move 2 from 6 to 4
move 3 from 4 to 2
move 8 from 9 to 4
move 8 from 4 to 8
move 7 from 2 to 1
move 5 from 8 to 7
move 2 from 2 to 3
move 13 from 1 to 2
move 2 from 3 to 8
move 2 from 9 to 7
move 3 from 8 to 1
move 2 from 1 to 2
move 2 from 8 to 4
move 6 from 7 to 2
move 3 from 1 to 8
move 1 from 7 to 5
move 24 from 2 to 1
move 2 from 8 to 5
move 15 from 1 to 4
move 1 from 5 to 8
move 9 from 1 to 4
move 2 from 8 to 5
move 26 from 2 to 4
move 1 from 5 to 8
move 1 from 5 to 8
move 50 from 4 to 1
move 1 from 8 to 9
move 1 from 4 to 6
move 1 from 4 to 9
move 22 from 1 to 5
move 1 from 6 to 2
move 1 from 5 to 8
move 1 from 2 to 4
move 1 from 8 to 1
move 28 from 1 to 3
move 2 from 9 to 4
move 21 from 5 to 8
move 1 from 1 to 8
move 1 from 5 to 8
move 1 from 5 to 7
move 3 from 4 to 8
move 1 from 7 to 9
move 1 from 9 to 7
move 20 from 8 to 4
move 2 from 8 to 1
move 1 from 7 to 6
move 2 from 1 to 4
move 27 from 3 to 1
move 4 from 8 to 4
move 1 from 6 to 9
move 19 from 4 to 2
move 5 from 2 to 5
move 1 from 4 to 1
move 1 from 9 to 2
move 17 from 1 to 9
move 1 from 3 to 8
move 15 from 9 to 2
move 2 from 4 to 8
move 2 from 5 to 8
move 2 from 5 to 9
move 3 from 9 to 8
move 9 from 1 to 2
move 2 from 1 to 3
move 4 from 4 to 5
move 2 from 5 to 7
move 1 from 8 to 5
move 2 from 3 to 8
move 4 from 5 to 2
move 1 from 9 to 6
move 5 from 8 to 5
move 1 from 7 to 9
move 29 from 2 to 3
move 1 from 8 to 6
move 1 from 9 to 7
move 2 from 2 to 8
move 2 from 5 to 2
move 2 from 7 to 5
move 4 from 5 to 9
move 1 from 5 to 9
move 10 from 3 to 4
move 10 from 4 to 7
move 1 from 3 to 4
move 5 from 2 to 9
move 5 from 8 to 6
move 1 from 6 to 5
move 2 from 6 to 3
move 4 from 6 to 7
move 1 from 5 to 2
move 2 from 2 to 7
move 5 from 7 to 8
move 8 from 7 to 2
move 6 from 8 to 7
move 14 from 2 to 5
move 3 from 7 to 3
move 1 from 4 to 7
move 2 from 7 to 2
move 3 from 2 to 8
move 3 from 8 to 5
move 8 from 9 to 1
move 3 from 7 to 2
move 2 from 7 to 4
move 17 from 3 to 6
move 8 from 1 to 6
move 16 from 5 to 2
move 1 from 5 to 2
move 1 from 3 to 1
move 21 from 6 to 7
move 1 from 4 to 8
move 7 from 7 to 8
move 1 from 1 to 3
move 11 from 7 to 2
move 7 from 2 to 6
move 8 from 8 to 5
move 2 from 7 to 4
move 4 from 5 to 6
move 8 from 2 to 8
move 17 from 2 to 3
move 4 from 5 to 3
move 7 from 6 to 9
move 2 from 6 to 9
move 1 from 4 to 1
move 1 from 4 to 2
move 3 from 6 to 2
move 1 from 6 to 8
move 1 from 4 to 1
move 1 from 7 to 5
move 10 from 9 to 2
move 1 from 5 to 6
move 1 from 8 to 2
move 1 from 1 to 4
move 12 from 3 to 4
move 1 from 6 to 2
move 2 from 8 to 6
move 1 from 1 to 2
move 1 from 9 to 8
move 2 from 8 to 7
move 6 from 3 to 2
move 1 from 3 to 5
move 8 from 4 to 9
move 22 from 2 to 9
move 7 from 3 to 5
move 3 from 8 to 2
move 2 from 7 to 8
move 3 from 6 to 9
move 1 from 2 to 9
move 1 from 6 to 2
move 4 from 8 to 5
move 5 from 5 to 9
move 1 from 3 to 6
move 1 from 5 to 6
move 2 from 4 to 1
move 2 from 2 to 4
move 4 from 4 to 6
move 1 from 1 to 5
move 5 from 6 to 3
move 35 from 9 to 1
move 4 from 9 to 1
move 1 from 4 to 7
move 3 from 3 to 7
move 37 from 1 to 7
move 2 from 2 to 3
move 3 from 3 to 7
move 1 from 5 to 8
move 2 from 1 to 8
move 2 from 5 to 2
move 1 from 6 to 9
move 16 from 7 to 1
move 5 from 1 to 5
move 3 from 8 to 2
move 10 from 7 to 9
move 6 from 7 to 9
move 3 from 2 to 1
move 4 from 5 to 3
move 2 from 1 to 2
move 5 from 7 to 9
move 5 from 7 to 9
move 5 from 5 to 3
move 8 from 3 to 7
move 6 from 9 to 4
move 8 from 7 to 3
move 2 from 3 to 6
move 1 from 6 to 7
move 1 from 6 to 7
move 5 from 4 to 9
move 3 from 7 to 1
move 2 from 2 to 8
move 1 from 8 to 6
move 6 from 1 to 8
move 1 from 7 to 9
move 1 from 3 to 9
move 4 from 3 to 2
move 8 from 1 to 6
move 1 from 3 to 9
move 5 from 8 to 4
move 2 from 3 to 1
move 1 from 8 to 2
move 4 from 9 to 1
move 2 from 1 to 5
move 1 from 8 to 5
move 11 from 9 to 5
move 1 from 2 to 8
move 10 from 5 to 4
move 1 from 1 to 9
move 3 from 5 to 4
move 5 from 2 to 3
move 1 from 5 to 1
move 9 from 9 to 4
move 1 from 6 to 7
move 1 from 3 to 9
move 4 from 3 to 1
move 1 from 2 to 4
move 1 from 1 to 4
move 1 from 4 to 7
move 5 from 1 to 3
move 1 from 3 to 2
move 1 from 8 to 3
move 3 from 9 to 5
move 1 from 2 to 9
move 4 from 1 to 4
move 1 from 7 to 4
move 2 from 5 to 8
move 1 from 7 to 6
move 4 from 3 to 1
move 1 from 5 to 8
move 1 from 3 to 4
move 22 from 4 to 1
move 11 from 1 to 9
move 2 from 1 to 4
move 11 from 1 to 6
move 8 from 6 to 7
move 1 from 8 to 7
move 7 from 9 to 2
move 6 from 7 to 6
move 2 from 4 to 9
move 2 from 7 to 1
move 14 from 6 to 3
move 2 from 3 to 1
move 3 from 6 to 7
move 6 from 1 to 3
move 8 from 9 to 6
move 7 from 4 to 6
move 7 from 6 to 8
move 1 from 9 to 1
move 2 from 9 to 8
move 4 from 3 to 4
move 1 from 8 to 4
move 1 from 4 to 3
move 6 from 3 to 7
move 7 from 2 to 5
move 8 from 4 to 6
move 1 from 7 to 2
move 1 from 5 to 7
move 6 from 7 to 3
move 1 from 7 to 1
move 8 from 8 to 4
move 8 from 4 to 2
move 3 from 7 to 3
move 6 from 5 to 6
move 15 from 3 to 1
move 21 from 6 to 1
move 4 from 2 to 6
move 5 from 6 to 5
move 1 from 2 to 6
move 1 from 4 to 5
move 1 from 4 to 3
move 1 from 8 to 6
move 4 from 5 to 7
move 18 from 1 to 4
move 2 from 5 to 7
move 6 from 7 to 6
move 1 from 3 to 2
move 6 from 1 to 2
move 3 from 3 to 9
move 3 from 9 to 4
move 1 from 8 to 3
move 1 from 6 to 5
move 6 from 2 to 5
move 1 from 5 to 9
move 1 from 3 to 5
move 2 from 6 to 8
move 2 from 1 to 4
move 5 from 4 to 6
move 15 from 4 to 9
move 5 from 9 to 1
move 2 from 6 to 2
move 6 from 6 to 3
move 1 from 8 to 6
move 6 from 5 to 9
move 3 from 6 to 5
move 2 from 4 to 7";
    }
}
