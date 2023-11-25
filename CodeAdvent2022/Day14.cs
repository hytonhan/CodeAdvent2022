using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CodeAdvent2022
{
    internal class Day14 : IDay
    {
        public int Order => 14;
        const string _inputFilename = "day14_input.txt";
        const string _testInputFilename = "day14_input_test.txt";

        private const int _startX = 500;
        private const int _startY = 0;
        private readonly Location _start;

        private Queue<(Location Location, char Symbol)> _drawQueue = new();

        public Day14()
        {
            _start = new Location(_startX, _startY);
        }

        public void Run()
        {
            Console.CursorVisible = false;
            List<List<Location>> allPaths = ParseInput(_inputFilename);
            List<Location> rocks = GetRockPositions(allPaths);

            var map = InitializeMap(rocks);
            var currCoord = _start;
            Location nextCoord = null;
            Part1(map, ref currCoord, ref nextCoord);

            map = InitializeMap2(rocks);
            currCoord = _start;
            nextCoord = null;
            Part2(map, ref currCoord, ref nextCoord);

        }

        private void Part2(MapPoint[,] map, ref Location currCoord, ref Location nextCoord)
        {
            while (true)
            {
                var currX = currCoord.X;
                var currY = currCoord.Y;

                var nextY = currY + 1;
                var nextX = currX;

                if (nextY >= map.GetLength(1))
                {
                    //Console.WriteLine($"Off map!");
                    //_drawQueue.Enqueue(new(currCoord, '.'));
                    break;
                }
                if(currX == _start.X && currY == _start.Y && map[currX, currY] == MapPoint.Sand)
                {
                    break;
                }

                var nextThing = map[nextX, nextY];
                if (nextThing == MapPoint.Rock || nextThing == MapPoint.Sand)
                {
                    nextX -= 1;
                    if (nextX < 0)
                    {
                        //Console.WriteLine($"Off map!");
                        //_drawQueue.Enqueue(new(currCoord, '.'));
                        break;
                    }
                    nextThing = map[nextX, nextY];
                    if (nextThing == MapPoint.Rock || nextThing == MapPoint.Sand)
                    {
                        nextX += 2;
                        if (nextX >= map.GetLength(0))
                        {
                            //Console.WriteLine($"Off map!");
                            //_drawQueue.Enqueue(new(currCoord, '.'));
                            break;
                        }
                        nextThing = map[nextX, nextY];
                        if (nextThing == MapPoint.Rock || nextThing == MapPoint.Sand)
                        {
                            nextX = currX;
                            nextY = currY;
                        }
                    }
                }

                nextCoord = new Location(nextX, nextY);

                if (nextCoord.X == currCoord.X && nextCoord.Y == currCoord.Y)
                {
                    map[currX, currY] = MapPoint.Sand;
                    //_drawQueue.Enqueue(new(currCoord, 'O'));
                    currCoord = _start;
                    if (nextCoord.X == _start.X && nextCoord.Y == _start.Y)
                    nextCoord = null;
                }
                else
                {
                    //_drawQueue.Enqueue(new(nextCoord, 'O'));
                    //_drawQueue.Enqueue(new(currCoord, '.'));
                    currCoord = nextCoord;
                    nextCoord = null;
                }

                //while (_drawQueue.Count > 0)
                //{
                //    var item = _drawQueue.Dequeue();
                //    Console.SetCursorPosition(item.Location.X, item.Location.Y);
                //    Console.Write(item.Symbol);

                //    Thread.Sleep(1);
                //}
            }

            //while (_drawQueue.Count > 0)
            //{
            //    var item = _drawQueue.Dequeue();
            //    Console.SetCursorPosition(item.Location.X, item.Location.Y);
            //    Console.Write(item.Symbol);
            //}

            Console.CursorVisible = true;
            var sum = 0;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == MapPoint.Sand)
                    {
                        sum++;
                    }
                }
            }
            //Console.SetCursorPosition(0, map.GetLength(1));
            Console.WriteLine($"Sum of sand: {sum}");
        }

        private void Part1(MapPoint[,] map, ref Location currCoord, ref Location nextCoord)
        {
            while (true)
            {
                var currX = currCoord.X;
                var currY = currCoord.Y;

                var nextY = currY + 1;
                var nextX = currX;

                if (nextY >= map.GetLength(1))
                {
                    //Console.WriteLine($"Off map!");
                    //_drawQueue.Enqueue(new(currCoord, '.'));
                    break;
                }

                var nextThing = map[nextX, nextY];
                if (nextThing == MapPoint.Rock || nextThing == MapPoint.Sand)
                {
                    nextX -= 1;
                    if (nextX < 0)
                    {
                        //Console.WriteLine($"Off map!");
                        //_drawQueue.Enqueue(new(currCoord, '.'));
                        break;
                    }
                    nextThing = map[nextX, nextY];
                    if (nextThing == MapPoint.Rock || nextThing == MapPoint.Sand)
                    {
                        nextX += 2;
                        if (nextX >= map.GetLength(0))
                        {
                            //Console.WriteLine($"Off map!");
                            //_drawQueue.Enqueue(new(currCoord, '.'));
                            break;
                        }
                        nextThing = map[nextX, nextY];
                        if (nextThing == MapPoint.Rock || nextThing == MapPoint.Sand)
                        {
                            nextX = currX;
                            nextY = currY;
                        }
                    }
                }

                nextCoord = new Location(nextX, nextY);

                if (nextCoord.X == currCoord.X && nextCoord.Y == currCoord.Y)
                {
                    map[currX, currY] = MapPoint.Sand;
                    //_drawQueue.Enqueue(new(currCoord, 'O'));
                    currCoord = _start;
                    nextCoord = null;
                }
                else
                {
                    //_drawQueue.Enqueue(new(nextCoord, 'O'));
                    //_drawQueue.Enqueue(new(currCoord, '.'));
                    currCoord = nextCoord;
                    nextCoord = null;
                }

                //while (_drawQueue.Count > 0)
                //{
                //    var item = _drawQueue.Dequeue();
                //    Console.SetCursorPosition(item.Location.X, item.Location.Y);
                //    Console.Write(item.Symbol);

                //    Thread.Sleep(10);
                //}
            }

            //while (_drawQueue.Count > 0)
            //{
            //    var item = _drawQueue.Dequeue();
            //    Console.SetCursorPosition(item.Location.X, item.Location.Y);
            //    Console.Write(item.Symbol);
            //}

            Console.CursorVisible = true;
            var sum = 0;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == MapPoint.Sand)
                    {
                        sum++;
                    }
                }
            }
            //Console.SetCursorPosition(0, map.GetLength(1));
            Console.WriteLine($"Sum of sand: {sum}");
        }

        private MapPoint[,] InitializeMap2(List<Location> rocks)
        {
            var minX = rocks.Min(x => x.X);
            var maxX = rocks.Max(x => x.X) + 1;
            var minY = rocks.Min(x => x.Y);
            var maxY = rocks.Max(x => x.Y) + 3;

            var map = new MapPoint[maxY * 2 + 2, maxY];
            var xDiff = maxX - minX;
            var center = maxY + 1;
            var padding = maxY - xDiff / 2;
            _start.X = center;

            for (int i = 0; i < maxY * 2 + 2; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    map[i, j] = MapPoint.Air;
                }
            }
            for (int i = 0; i < maxY * 2 + 2; i++)
            {
                map[i, maxY-1] = MapPoint.Rock;
            }
            foreach (var rock in rocks)
            {
                map[rock.X - 500 + center, rock.Y] = MapPoint.Rock;
            }
            map[center, 0] = MapPoint.Start;

            //Console.Clear();

            for (int i = 0; i < maxY; i++)
            {
                for (int j = 0; j < maxY * 2 + 2; j++)
                {
                    char symbol = '.';
                    switch (map[j, i])
                    {
                        case MapPoint.Air:
                            symbol = '.';
                            break;
                        case MapPoint.Rock:
                            symbol = '#';
                            break;
                        case MapPoint.Sand:
                            symbol = 'O';
                            break;
                        case MapPoint.Start:
                            symbol = '+';
                            break;
                    }
                    //Console.Write(symbol);
                }
                //Console.Write('\n');
            }
            return map;
        }

        private MapPoint[,] InitializeMap(List<Location> rocks)
        {
            var minX = rocks.Min(x => x.X);
            var maxX = rocks.Max(x => x.X);
            var minY = rocks.Min(x => x.Y);
            var maxY = rocks.Max(x => x.Y);
            _start.X -= minX;
            var map = new MapPoint[maxX - minX + 1, maxY + 1];

            for (int i = 0; i < maxX - minX + 1; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    map[i, j] = MapPoint.Air;
                }
            }
            foreach (var rock in rocks)
            {
                map[rock.X - minX, rock.Y] = MapPoint.Rock;
            }
            map[500 - minX, 0] = MapPoint.Start;

            //Console.Clear();

            for (int i = 0; i < maxY + 1; i++)
            {
                for (int j = 0; j < maxX - minX + 1; j++)
                {
                    char symbol = '.';
                    switch (map[j, i])
                    {
                        case MapPoint.Air:
                            symbol = '.';
                            break;
                        case MapPoint.Rock:
                            symbol = '#';
                            break;
                        case MapPoint.Sand:
                            symbol = 'O';
                            break;
                        case MapPoint.Start:
                            symbol = '+';
                            break;
                    }
                    //Console.Write(symbol);
                }
                //Console.Write('\n');
            }
            return map;
        }

        private List<Location> GetRockPositions(List<List<Location>> allPaths)
        {
            var rocks = new List<Location>();
            foreach (var path in allPaths)
            {
                Location first = null;
                Location second = null;
                foreach (var location in path)
                {
                    second = first;
                    first = location;
                    if (second == null)
                    {
                        rocks.Add(first);
                        continue;
                    }

                    var xDiff = first.X - second.X;
                    var yDiff = first.Y - second.Y;

                    if (xDiff > 0)
                    {
                        for (int i = 0; i < Math.Abs(xDiff); i++)
                        {
                            rocks.Add(new Location(second.X - (i - xDiff), second.Y));
                        }
                    }
                    else if (xDiff < 0)
                    {
                        for (int i = 0; i < Math.Abs(xDiff); i++)
                        {
                            rocks.Add(new Location(second.X - i - 1, second.Y));
                        }
                    }
                    if (yDiff > 0)
                    {
                        for (int i = 0; i < Math.Abs(yDiff); i++)
                        {
                            rocks.Add(new Location(second.X, second.Y - (i - yDiff)));
                        }
                    }
                    else if (yDiff < 0)
                    {
                        for (int i = 0; i < Math.Abs(yDiff); i++)
                        {
                            rocks.Add(new Location(second.X, second.Y - i - 1));
                        }
                    }

                }
                first = null;
                second = null;
            }
            return rocks;
        }

        private List<List<Location>> ParseInput(string filename)
        {
            string[] lines = File.ReadAllLines(filename);

            var allPaths = new List<List<Location>>();
            foreach (var line in lines)
            {
                List<Location> path = new();
                var parts = line.Split(" -> ");
                foreach (var part in parts)
                {
                    var coords = part.Split(',');
                    path.Add(new Location(int.Parse(coords[0]), int.Parse(coords[1])));
                }
                allPaths.Add(path);
            }
            return allPaths;
        }
    }

    enum MapPoint
    {
        Air,
        Rock,
        Sand,
        Start
    }

    class Location
    {
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
