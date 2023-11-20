using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    internal class Day06 : IDay
    {
        public int Order => 6;
        const string _inputFilename = "day06_input.txt";

        public void Run()
        {
            string[] lines = File.ReadAllLines(_inputFilename);

            Queue<char> queue = new();

            int index = 0;
            foreach (char @char in lines.First())
            {
                index++;
                queue.Enqueue(@char);
                if (queue.Count() < 4)
                {
                    continue;
                }

                var foo = new char[4];
                queue.CopyTo(foo, 0);

                var uniqueChars = foo.Distinct().Count();
                if(uniqueChars == 4)
                {
                    break;
                }
                queue.Dequeue();
            }

            queue.Clear();
            int index2 = 0;
            foreach (char @char in lines.First())
            {
                index2++;
                queue.Enqueue(@char);
                if (queue.Count() < 14)
                {
                    continue;
                }

                var foo = new char[14];
                queue.CopyTo(foo, 0);

                var uniqueChars = foo.Distinct().Count();
                if(uniqueChars == 14)
                {
                    break;
                }
                queue.Dequeue();
            }

            Console.WriteLine($"start of packet: {index}");
            Console.WriteLine($"start of message: {index2}");
        }
    }
}
