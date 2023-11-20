using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    internal class Day09 : IDay
    {
        public int Order => 9;

        const string _inputFilename = "day09_input.txt";

        public void Run()
        {
            (int x, int y) head = new(0, 0);
            (int x, int y) tail = new(0, 0);

            List<(int x, int y)> rope = new()
            {
                (0, 0),
                (0, 0),
                (0, 0),
                (0, 0),
                (0, 0),
                (0, 0),
                (0, 0),
                (0, 0),
                (0, 0),
                (0, 0)
            };

            List<(int x, int y)> visitedCoords = new()
            {
                (0, 0)
            };
            List<(int x, int y)> visitedCoords2 = new()
            {
                (0, 0)
            };

            string[] lines = File.ReadAllLines(_inputFilename);
            foreach (string line in lines)
            {
                var parts = line.Split(' ');
                var command = parts[0];
                var count = int.Parse(parts[1]);

                switch (command)
                {
                    case "R":
                        for (int i = 0; i < count; i++)
                        {
                            head.x++;
                            Foo(head, ref tail, 1);
                            if(!visitedCoords.Contains(tail))
                            {
                                visitedCoords.Add(new (tail.x, tail.y));
                            }

                            rope[0] = (rope[0].x + 1, rope[0].y);
                            var previous = rope.First();
                            for(int j = 1; j < 10; j++)
                            {
                                var temp = rope[j];
                                rope[j] = Foo(previous, ref temp, 1);
                                previous = rope[j];
                            }
                            if (!visitedCoords2.Contains(rope[9]))
                            {
                                visitedCoords2.Add(new(rope[9].x, rope[9].y));
                            }
                        }
                        break;
                    case "L":
                        for (int i = 0; i < count; i++)
                        {
                            head.x--;
                            Foo(head, ref tail, 1);
                            if (!visitedCoords.Contains(tail))
                            {
                                visitedCoords.Add(new(tail.x, tail.y));
                            }

                            rope[0] = (rope[0].x - 1, rope[0].y);
                            var previous = rope.First();
                            for (int j = 1; j < 10; j++)
                            {
                                var temp = rope[j];
                                rope[j] = Foo(previous, ref temp, 1);
                                previous = rope[j];
                            }
                            if (!visitedCoords2.Contains(rope[9]))
                            {
                                visitedCoords2.Add(new(rope[9].x, rope[9].y));
                            }
                        }
                        break;
                    case "U":
                        for (int i = 0; i < count; i++)
                        {
                            head.y++;
                            Foo(head, ref tail, 1);
                            if (!visitedCoords.Contains(tail))
                            {
                                visitedCoords.Add(new(tail.x, tail.y));
                            }

                            rope[0] = (rope[0].x, rope[0].y +1);
                            var previous = rope.First();
                            for (int j = 1; j < 10; j++)
                            {
                                var temp = rope[j];
                                rope[j] = Foo(previous, ref temp, 1);
                                previous = rope[j];
                            }
                            if (!visitedCoords2.Contains(rope[9]))
                            {
                                visitedCoords2.Add(new(rope[9].x, rope[9].y));
                            }
                        }
                        break;
                    case "D":
                        for (int i = 0; i < count; i++)
                        {
                            head.y--;
                            Foo(head, ref tail, 1);
                            if (!visitedCoords.Contains(tail))
                            {
                                visitedCoords.Add(new(tail.x, tail.y));
                            }

                            rope[0] = (rope[0].x, rope[0].y - 1);
                            var previous = rope.First();
                            for (int j = 1; j < 10; j++)
                            {
                                var temp = rope[j];
                                rope[j] = Foo(previous, ref temp, 1);
                                previous = rope[j];
                            }
                            if (!visitedCoords2.Contains(rope[9]))
                            {
                                visitedCoords2.Add(new(rope[9].x, rope[9].y));
                            }
                        }
                        break;
                }
            }

            Console.WriteLine($"Visited coords: {visitedCoords.Count()}");
            Console.WriteLine($"Visited coords2: {visitedCoords2.Count()}");
        }

        public (int x, int y) Foo((int x, int y) head, ref (int x, int y) tail, int maxDistance)
        {
            int foo = Math.Abs(tail.y - head.y);
            int bar = Math.Abs(tail.x - head.x);
            if (head.x != tail.x && head.y != tail.y && (foo > maxDistance || bar > maxDistance))
            {
                int yDiff = tail.y > head.y ? 1 : -1;
                int xDiff = tail.x > head.x ? 1 : -1;
                tail.y -= yDiff;
                tail.x -= xDiff;
                return tail;
            }

            if ((head.x - tail.x) > maxDistance)
            {
                tail.x++;
            }
            else if ((head.x - tail.x) < (maxDistance * -1))
            {
                tail.x--;
            }
            if ((head.y - tail.y) > maxDistance)
            {
                tail.y++;
            }
            else if ((head.y - tail.y) < (maxDistance * -1))
            {
                tail.y--;
            }
            return tail;
        }
    }

}
