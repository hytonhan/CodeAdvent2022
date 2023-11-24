using Newtonsoft.Json;
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
            string[] lines = File.ReadAllLines(_inputFilename);

            List<int> indices = new();
            List<int> values = new();

            List<List<object>> packets = new();

            for (int i = 0; i < lines.Length; i += 3)
            {
                int index = i / 3 + 1;
                var first = lines[i];
                var second = lines[i + 1];

                var packet1 = ParseList(new Queue<char>(first.ToCharArray()));
                var packet2 = ParseList(new Queue<char>(second.ToCharArray()));

                packets.Add(packet1);
                packets.Add(packet2);

                int result = Compare(packet1, packet2);

                string foo = result == -1 ? "Not in order" : "in order";

                if (result >= 0)
                {
                    indices.Add(index);
                    values.Add(result);
                }
                //Console.WriteLine($"Packet index: {index}. Result: {foo}");
            }

            var sum = indices.Sum();
            Console.WriteLine($"Sum of indices of ordered packets: {sum}");

            List<object> divider1 = new List<object>()
            {
                new List<object>()
                {
                    2
                }
            };
            List<object> divider2 = new List<object>()
            {
                new List<object>()
                {
                    6
                }
            };

            packets.Add(divider1);
            packets.Add(divider2);
            packets.Sort(Compare);
            packets.Reverse();

            var index1 = packets.IndexOf(divider1) + 1;
            var index2 = packets.IndexOf(divider2) + 1;

            Console.WriteLine($"Product of divider indices: {index1 * index2}");
        }

        private int Compare(List<object> packet1, List<object> packet2)
        {
            if (packet1.Count == 0 && packet2.Count > 0)
            {
                return 1;
            }
            for (int i = 0; i < packet1.Count; i++)
            {
                if (i >= packet2.Count)
                {
                    return -1;
                }
                var item1 = packet1[i];
                var item2 = packet2[i];

                if (item1.GetType() == typeof(List<object>) && item2.GetType() == typeof(List<object>))
                {
                    var comp = Compare((List<object>)item1, (List<object>)item2);
                    if (comp != 0)
                    {
                        return comp;
                    }
                    continue;
                }

                if (item1.GetType() == typeof(int) && item2.GetType() == typeof(int))
                {
                    if ((int)item1 < (int)item2)
                    {
                        return 1;
                    }
                    else if ((int)item1 > (int)item2)
                    {
                        return -1;
                    }
                    continue;
                }
                if (item1.GetType() == typeof(int))
                {
                    var foo = new List<object>()
                    {
                        (int)item1
                    };
                    var comp = Compare((List<object>)foo, (List<object>)item2);
                    if (comp != 0)
                    {
                        return comp;
                    }
                }
                else if (item2.GetType() == typeof(int))
                {
                    var foo = new List<object>()
                    {
                        (int)item2
                    };
                    var comp = Compare((List<object>)item1, (List<object>)foo);
                    if (comp != 0)
                    {
                        return comp;
                    }
                }
            }
            if (packet1.Count < packet2.Count)
            {
                return 1;
            }
            return 0;

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
                    int number = (int)char.GetNumericValue(input.Dequeue());
                    if (input.Peek() == '0')
                    {
                        number *= 10;
                        input.Dequeue();
                    }
                    temp.Add(number);
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
