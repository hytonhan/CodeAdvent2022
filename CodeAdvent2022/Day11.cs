using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    internal class Day11 : IDay
    {
        public static List<Monkey> Monkeys = new();

        public int Order => 11;

        const string _inputFilename = "day11_input.txt";

        public void Run()
        {
            Monkeys.Clear();
            Monkeys.Add(new Monkey(0, "*", "5", 11, new int[] { 83, 88, 96, 79, 86, 88, 70 }, 2, 3));
            Monkeys.Add(new Monkey(1, "*", "11", 5, new int[] { 59, 63, 98, 85, 68, 72 }, 4, 0));
            Monkeys.Add(new Monkey(2, "+", "2", 19, new int[] { 90, 79, 97, 52, 90, 94, 71, 70 }, 5, 6));
            Monkeys.Add(new Monkey(3, "+", "5", 13, new int[] { 97, 55, 62 }, 2, 6));
            Monkeys.Add(new Monkey(4, "*", "old", 7, new int[] { 74, 54, 94, 76 }, 0, 3));
            Monkeys.Add(new Monkey(5, "+", "4", 17, new int[] { 58 }, 7, 1));
            Monkeys.Add(new Monkey(6, "+", "6", 2, new int[] { 66, 63 }, 7, 5));
            Monkeys.Add(new Monkey(7, "+", "7", 3, new int[] { 56, 56, 90, 96, 68 }, 4, 1));
            var leastCommonMultiple = 1;
            foreach (var monkey in Monkeys)
            {
                leastCommonMultiple = lcm(monkey.TestValue, leastCommonMultiple);
            }
            Console.WriteLine($"lcm: {leastCommonMultiple}");
            for (int i = 0; i < 20; i++)
            {
                foreach (var monkey in Monkeys.OrderBy(x => x.Id))
                {
                    monkey.InspectAll(true, leastCommonMultiple);
                }
            }
            foreach (var monkey in Monkeys.OrderBy(x => x.Id))
            {
                Console.WriteLine($"Monkey {monkey.Id} inspect count: {monkey.InspectCount}");
            }
            int monkeyBusiness = 1;
            foreach (var monkey in Monkeys.OrderByDescending(x => x.InspectCount).Take(2))
            {
                monkeyBusiness *= monkey.InspectCount;
            }
            Console.WriteLine($"Monkey business score: {monkeyBusiness}");

            Monkeys.Clear();
            Monkeys.Add(new Monkey(0, "*", "5", 11, new int[] { 83, 88, 96, 79, 86, 88, 70 }, 2, 3));
            Monkeys.Add(new Monkey(1, "*", "11", 5, new int[] { 59, 63, 98, 85, 68, 72 }, 4, 0));
            Monkeys.Add(new Monkey(2, "+", "2", 19, new int[] { 90, 79, 97, 52, 90, 94, 71, 70 }, 5, 6));
            Monkeys.Add(new Monkey(3, "+", "5", 13, new int[] { 97, 55, 62 }, 2, 6));
            Monkeys.Add(new Monkey(4, "*", "old", 7, new int[] { 74, 54, 94, 76 }, 0, 3));
            Monkeys.Add(new Monkey(5, "+", "4", 17, new int[] { 58 }, 7, 1));
            Monkeys.Add(new Monkey(6, "+", "6", 2, new int[] { 66, 63 }, 7, 5));
            Monkeys.Add(new Monkey(7, "+", "7", 3, new int[] { 56, 56, 90, 96, 68 }, 4, 1));

            //Monkeys.Add(new Monkey(0, "*", "19", 23, new int[] { 79, 98 }, 2, 3));
            //Monkeys.Add(new Monkey(1, "+", "6", 19, new int[] { 54, 65, 75, 74 }, 2, 0));
            //Monkeys.Add(new Monkey(2, "*", "old", 13, new int[] { 79, 60, 97 }, 1, 3));
            //Monkeys.Add(new Monkey(3, "+", "3", 17, new int[] { 74 }, 0, 1));

            leastCommonMultiple = 1;
            foreach(var monkey in Monkeys)
            {
                leastCommonMultiple = lcm(monkey.TestValue, leastCommonMultiple);
            }
            Console.WriteLine($"lcm: {leastCommonMultiple}");

            for (int i = 0; i < 10000; i++)
            {
                foreach (var monkey in Monkeys.OrderBy(x => x.Id))
                {
                    monkey.InspectAll(false, leastCommonMultiple);
                }

                //if (i + 1 == 1 || 
                //    i + 1 == 20 || 
                //    i + 1 == 1000 || 
                //    i + 1 == 2000 || 
                //    i + 1 == 3000 || 
                //    i + 1 == 4000 || 
                //    i + 1 == 5000 || 
                //    i + 1 == 6000 || 
                //    i + 1 == 7000 || 
                //    i + 1 == 8000 || 
                //    i + 1 == 9000 || 
                //    i + 1 == 10000)
                //{
                //    Console.WriteLine($"== After round {i + 1} ==");
                //    foreach (var monkey in Monkeys.OrderBy(x => x.Id))
                //    {
                //        Console.WriteLine($"Monkey {monkey.Id} inspected items {monkey.InspectCount} times.");
                //    }
                //}
            }
            foreach (var monkey in Monkeys.OrderBy(x => x.Id))
            {
                Console.WriteLine($"Monkey {monkey.Id} inspect count: {monkey.InspectCount}");
            }
            long monkeyBusiness2 = 1;
            foreach (var monkey in Monkeys.OrderByDescending(x => x.InspectCount).Take(2))
            {
                monkeyBusiness2 *= monkey.InspectCount;
            }



            Console.WriteLine($"Monkey business without relief score: {monkeyBusiness2}");
        }
        static int gcf(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static int lcm(int a, int b)
        {
            return (a / gcf(a, b)) * b;
        }

    }

    internal class Monkey
    {
        private string _operator = "";
        private string _operationValue = "";

        public int TestValue;
        private int _targetIfTestTrue;
        private int _targetIfTestFalse;

        public int InspectCount { get; set; }

        public Monkey(int id, string @operator, string operationValue, int testValue, int[] startingItems, int targetTrue, int targetFalse)
        {
            Id = id;
            _operator = @operator;
            _operationValue = operationValue;
            TestValue = testValue;
            Items = new Queue<Item>();
            foreach (var item in startingItems)
            {
                Items.Enqueue(new Item() { WorryLevel = item });
            }
            _targetIfTestTrue = targetTrue;
            _targetIfTestFalse = targetFalse;
            InspectCount = 0;
        }

        public int Id { get; private set; }

        public Queue<Item> Items { get; set; }

        public void InspectAll(bool reliefActive, int lcm)
        {
            for (var i = Items.Count(); i > 0; i--)
            {
                var item = Items.Dequeue();
                Inspect(item, reliefActive, lcm);
            }
        }


        private void Operation(Item item, int lcm)
        {
            try
            {
                checked
                {
                    switch (_operator)
                    {
                        case "+":
                            if (_operationValue == "old")
                            {
                                item.WorryLevel += item.WorryLevel;
                            }
                            else
                            {
                                item.WorryLevel += int.Parse(_operationValue);
                            }
                            break;
                        case "*":
                            if (_operationValue == "old")
                            {
                                item.WorryLevel = (item.WorryLevel % lcm) * (item.WorryLevel % lcm);
                            }
                            else
                            {
                                item.WorryLevel *= int.Parse(_operationValue);
                            }
                            break;
                    }
                    if (item.WorryLevel > lcm)
                    {
                        item.WorryLevel = (item.WorryLevel % lcm) + lcm;
                    }
                }
            }
            catch (OverflowException e)
            {
                Console.WriteLine(e.Message);  // output: Arithmetic operation resulted in an overflow.
            }
        }

        private bool Test(Item item)
        {
            return item.WorryLevel % TestValue == 0;
        }

        private void Throw(Item item, int target)
        {
            var monkey = Day11.Monkeys.Where(x => x.Id == target).Single();
            monkey.GiveItem(item);
        }

        private void Relief(Item item)
        {
            item.WorryLevel = item.WorryLevel / 3;
        }

        private void Inspect(Item item, bool reliefActive, int lcm)
        {
            InspectCount++;
            Operation(item, lcm);
            if (reliefActive)
            {
                Relief(item);
            }
            if (Test(item))
            {
                Throw(item, _targetIfTestTrue);
            }
            else
            {
                Throw(item, _targetIfTestFalse);
            }
        }

        private void GiveItem(Item item)
        {
            Items.Enqueue(item);
        }
    }

    internal class Item
    {
        public long WorryLevel { get; set; }
    }
}
