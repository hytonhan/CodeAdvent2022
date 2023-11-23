using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    internal class Day13 : IDay
    {
        public int Order => 13;
        const string _inputFilename = "day13_input.txt";

        public void Run()
        {
            string[] lines = File.ReadAllLines(_inputFilename);

            for (int i = 0; i < lines.Length; i += 3)
            {
                var first = lines[i];
                var second = lines[i + 1];

                //var foo = ParseList(new Queue<char>(first.ToCharArray()));
            }
        }

        //private List<object> ParseList(Queue<char> input)
        //{
        //    List<object> temp = new();
        //    input.Dequeue();

        //}

        //private object ParseElement(Queue<char> input)
        //{

        //}

        private void ParseInput(string input, string second)
        {

            var listStart = input.LastIndexOf('[');
            var listEnd = input.IndexOf(']', listStart);


            var temp = new List<object>();
            var integers = input.Substring(listStart, listEnd).Split(',');
            foreach(var number in integers)
            {
                temp.Add(int.Parse(number));
            }
            input.Remove(listStart, listEnd);
        }

    }

}
