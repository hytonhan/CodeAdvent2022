using SimpleInjector;
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
        const string _testInputFilename = "day13_input_test.txt";

        public void Run()
        {
            string[] lines = File.ReadAllLines(_testInputFilename);

            for (int i = 0; i < lines.Length; i += 3)
            {
                var first = lines[i];
                var second = lines[i + 1];

                var packet1 = ParseList(new Queue<char>(first.ToCharArray()));
                var packet2 = ParseList(new Queue<char>(second.ToCharArray()));

                Compare(packet1, packet2);
            }
        }

        private bool Compare(List<object> packet1, List<object> packet2)
        {

            for (int i = 0; i <= packet1.Count; i++)
            {
                if (i >= packet1.Count)
                {
                    return false;
                }
                var item1 = packet1[i];
                var item2 = packet2[i];

                if(item1.GetType() == typeof(List<object>) && item2.GetType() == typeof(List<object>))

                if (item1.GetType() == typeof(int) && item2.GetType() == typeof(int))
                {
                    if ((int)item1 < (int)item2)
                    {
                        return true;
                    }
                    else if ((int)item1 > (int)item2)
                    {
                        return false;
                    }
                }
            }
            return true;

        }

        private List<object> ParseList(Queue<char> input)
        {
            List<object> temp = new();
            input.Dequeue();

            while (input.Count > 0)
            {
                if (input.Peek() == '[')
                {
                    var subList = ParseList(input);
                    temp.Add(subList);
                }
                if (input.Peek() == ']')
                {
                    input.Dequeue();
                    break;
                }
                if (input.Peek() == ',')
                {
                    input.Dequeue();
                    continue;
                }
                else
                {
                    temp.Add(char.GetNumericValue(input.Dequeue()));
                }
            }

            return temp;
        }


        private void ParseInput(string input, string second)
        {

            var listStart = input.LastIndexOf('[');
            var listEnd = input.IndexOf(']', listStart);


            var temp = new List<object>();
            var integers = input.Substring(listStart, listEnd).Split(',');
            foreach (var number in integers)
            {
                temp.Add(int.Parse(number));
            }
            input.Remove(listStart, listEnd);
        }

    }

}
