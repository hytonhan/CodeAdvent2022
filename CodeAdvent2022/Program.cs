using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                typeof(Day06)
            };
            _container.Collection.Register<IDay>(days);

            _container.Verify();
        }

        public static void Main()
        {
            Console.WriteLine("Hello world!");

            var type = typeof(IDay);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            var days = _container.GetAllInstances<IDay>();

            var stopwatch = new Stopwatch();
            while (true)
            {
                Console.Write($"Select a number between 1 and {days.Count()}, or type all to print all days. -1 to exit. ");
                var input = Console.ReadLine();

                if (input == "all")
                {
                    stopwatch.Restart();
                    foreach (var day in days.OrderBy(x => x.Order))
                    {
                        Console.WriteLine($"day {day.Order}");
                        day.Run();
                    }
                    stopwatch.Stop();
                    Console.WriteLine($"Execution time: {stopwatch.ElapsedMilliseconds} ms");
                    continue;
                }
                if (!int.TryParse(input, out int result))
                {
                    Console.WriteLine($"Idiot.");
                    continue;
                }
                if (result >= 1 && result <= days.Count())
                {
                    var day = days.Where(x => x.Order == result)
                                  .Single();
                    stopwatch.Restart();
                    day.Run();
                    stopwatch.Stop();
                    Console.WriteLine($"Execution time: {stopwatch.ElapsedMilliseconds} ms");
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
