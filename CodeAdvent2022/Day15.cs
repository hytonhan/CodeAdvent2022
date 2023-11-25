using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    internal class Day15 : IDay
    {
        public int Order => 15;

        const string _inputFilename = "day15_input.txt";

        public void Run()
        {
            string[] lines = File.ReadAllLines(_inputFilename);
        }
    }
}
