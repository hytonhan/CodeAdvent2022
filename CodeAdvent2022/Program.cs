using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace CodeAdvent2022
{
    public class Program
    {
        static readonly Container _container;

        static Program()
        {
            _container = new Container();

            var days = new List<Type>()
            {
                typeof(Day01),
                typeof(Day02),
                typeof(Day03),
                typeof(Day04),
                typeof(Day05),
                typeof(Day06),
                typeof(Day07),
                typeof(Day08),
                typeof(Day09),
                typeof(Day10),
                typeof(Day11),
                typeof(Day12),
                typeof(Day13),
                typeof(Day14),
                typeof(Day15),
                typeof(Day16)
            };
            _container.Collection.Register<IDay>(days, Lifestyle.Transient);

            _container.Verify();
        }

        public static void Main()
        {
            Console.ForegroundColor = ConsoleColor.White;

            //var days = _container.GetAllInstances<IDay>();

            var stopwatch = new Stopwatch();
            while (true)
            {
                Console.Write($"Select a number between 1 and {_container.GetAllInstances<IDay>().Count()}, or type all to print all days. -1 to exit. ");
                var input = Console.ReadLine();

                if (input == "all")
                {
                    var dayStopwatch = new Stopwatch();
                    stopwatch.Restart();
                    foreach (var day in _container.GetAllInstances<IDay>().OrderBy(x => x.Order))
                    {
                        dayStopwatch.Restart();
                        Console.WriteLine($"day {day.Order}");
                        day.Run();
                        Console.WriteLine($"Execution time for day: {dayStopwatch.ElapsedMilliseconds} ms.");
                    }
                    stopwatch.Stop();
                    Console.WriteLine($"Total execution time: {stopwatch.ElapsedMilliseconds} ms");
                    continue;
                }
                if (!int.TryParse(input, out int result))
                {
                    Console.WriteLine($"Idiot.");
                    continue;
                }
                if (result >= 1 && result <= _container.GetAllInstances<IDay>().Count())
                {
                    var days = _container.GetAllInstances<IDay>();
                    var day = days.Where(x => x.Order == result).Single();
                    stopwatch.Restart();
                    day.Run();
                    stopwatch.Stop();
                    var time = stopwatch.ElapsedMilliseconds < 2 ? $"{stopwatch.ElapsedTicks / 10} us" : $"{stopwatch.ElapsedMilliseconds} ms";
                    Console.WriteLine($"Execution time: {time}");
                    continue;
                }
                if (result == -1)
                {
                    break;
                }
                Console.WriteLine($"wrong number.");
            }
        }
    }
}
