using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CodeAdvent2022
{
    internal class Day12 : IDay
    {
        public int Order => 12;
        const string _inputFilename = "day12_input.txt";
        const string _testInputFilename = "day12_input_test.txt";

        public void Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.ForegroundColor = ConsoleColor.White;
            string[] lines = File.ReadAllLines(_inputFilename);
            //string[] lines = File.ReadAllLines(_testInputFilename);

            Point start = null;
            Point end = null;
            int width = lines.First().Length;
            int height = lines.Length;
            List<Point> points = new List<Point>();

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == 'S')
                    {
                        start = new Point(j, i, 'a' - 'a');
                        points.Add(start);
                    }
                    else if (lines[i][j] == 'E')
                    {
                        end = new Point(j, i, 'z' - 'a');
                        points.Add(end);
                    }
                    else
                    {
                        var temp = new Point(j, i, lines[i][j] - 'a');
                        points.Add(temp);
                    }
                }
            }

            bool drawFirst = false;
            var search = new SearchClass(points, start, end, width, height, drawFirst);
            Console.CursorVisible = false;
            var path = search.FindPath();
            Console.CursorVisible = true;

            if (drawFirst)
            {
                Console.SetCursorPosition(0, height);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"");
            Console.WriteLine($"Steps: {path.Count()}. Execution time: {stopwatch.ElapsedMilliseconds} ms");

            //PART 2
            stopwatch.Restart();
            List<int> paths = new();
            int startPosCount = points.Where(x => x.Height == 0).Count();
            Console.WriteLine($"\nstart positions: {startPosCount}");
            int counter = 1;
            Console.Write($"Processing ");
            var pos = Console.GetCursorPosition();
            search.ShouldDraw = false;
            foreach (var startPoint in points.Where(x => x.Height == 0))
            {
                Console.SetCursorPosition(pos.Left, pos.Top);
                Console.Write($"{counter}/{startPosCount}.");

                search.OpenList = new();
                foreach (var list in search.Nodes)
                {
                    foreach (var node in list)
                    {
                        node.State = NodeState.Untested;
                        node.ParentNode = null;
                        if (node.Location == startPoint)
                        {
                            search.StartNode = node;
                        }
                        if (node.Location == end)
                        {
                            search.EndNode = node;
                        }
                        var heightDiff = end.Height - node.Location.Height;
                        var manhattanDistance = Math.Abs(node.Location.X - end.X) + Math.Abs(node.Location.Y - end.Y);
                        node.H = manhattanDistance + heightDiff * 100;
                    }
                }

                int steps = search.FindPath().Count();
                paths.Add(steps);
                Console.WriteLine($" - steps: {steps}");
                counter++;
            }

            stopwatch.Stop();
            var avg = (float)stopwatch.ElapsedMilliseconds / (float)startPosCount;
            Console.WriteLine($"Shortest path from height 'a': {paths.Where(x => x != 0).OrderBy(x => x).First()}. Execution time (part 2): {stopwatch.ElapsedMilliseconds} ms. Avg: {avg}.");
        }
    }

    internal class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Height { get; set; }

        public Point(int x, int y, int height)
        {
            X = x;
            Y = y;
            Height = height;
        }

    }

    internal class DrawBuffer
    {
        public DrawBuffer(int width, int height)
        {
            Width = width;
            Height = height;

        }

        public int Width { get; set; }

        public int Height { get; set; }
    }

    internal class SearchClass
    {
        public SearchClass(List<Point> points, Point start, Point end, int width, int height, bool drawPath)
        {
            Nodes = new List<List<Node>>();
            OpenList = new PriorityQueue<Node, double>();
            var temp = new List<Node>();
            foreach (Point p in points)
            {
                if(p.X == 0)
                {
                    temp = new List<Node>();
                }
                if (p == start)
                {
                    var startNode = new Node(p, null);
                    StartNode = startNode;
                    temp.Add(StartNode);
                }
                else if (p == end)
                {
                    var endNode = new Node(end, null);
                    EndNode = endNode;
                    temp.Add(EndNode);
                }
                else
                {
                    temp.Add(new Node(p, end));
                }
                if (p.X + 1 == width)
                {
                    Nodes.Add(temp);
                }
            }
            Width = width;
            Height = height;
            ShouldDraw = drawPath;

            CharMap = new char[width, height];
            DrawQueue = new();
        }

        public char[,] CharMap { get; set; }

        private Queue<(int x, int y, char @char)> DrawQueue { get; set; }

        public bool ShouldDraw { get; set; }

        public Node StartNode { get; set; }
        public Node EndNode { get; set; }

        public List<List<Node>> Nodes { get; set; }

        public PriorityQueue<Node, double> OpenList { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }


        public List<Point> FindPath()
        {
            List<Point> path = new List<Point>();
            if (ShouldDraw)
            {
                ClearGrid();
            }
            bool success = Search(StartNode, ShouldDraw);
            if (success)
            {
                Node node = this.EndNode;
                while (node.ParentNode != null)
                {
                    path.Add(node.Location);
                    node = node.ParentNode;
                }
                path.Reverse();
            }
            if (ShouldDraw)
            {
                //DrawPath(EndNode);
                while (DrawQueue.Count > 0)
                {
                    var temp = DrawQueue.Dequeue();
                    if (temp.@char == '<' || temp.@char == '>' || temp.@char == 'v' || temp.@char == '^')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.SetCursorPosition(temp.x, temp.y);
                    Console.Write(temp.@char);
                }
            }
            return path;
        }

        private void ClearGrid()
        {
            string colors = @"$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\|()1{}[]?-_+~<>i!lI;:,"" ^`'.";
            var arr = colors.ToCharArray();
            Array.Reverse(arr);
            colors = new string(arr);

            var len = colors.Length -1;

            int min = 'a' - 'a';
            int max = 'z' - 'a';
            float temp = (float)len / max;

            foreach(var list in Nodes)
            {
                foreach (var node in list)
                {
                    Console.SetCursorPosition(node.Location.X, node.Location.Y);
                    if (node == EndNode)
                    {
                        //Console.Write('E');
                        CharMap[node.Location.X, node.Location.Y] = 'E';
                        continue;
                    }
                    var @char = colors[(int)(node.Location.Height * temp)];
                    CharMap[node.Location.X, node.Location.Y] = @char;
                    //Console.Write(@char);
                }
            }
            for(int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(0, i);
                for(int j = 0; j < Width; j++)
                {
                    Console.Write(CharMap[j, i]);
                }
            }
        }

        private void DrawPath(Node currentNode, Node nextNode)
        {
            var parent = nextNode.ParentNode;
            var current = nextNode;
            var path = new List<(Point, char)>();
            while(parent != null)
            {
                //Console.SetCursorPosition(parent.Location.X, parent.Location.Y);
                char symbol = '.';
                if ((current.Location.X - parent.Location.X) == 1)
                {
                    symbol = '>';
                }
                else if ((current.Location.X - parent.Location.X) == -1)
                {
                    symbol = '<';
                }
                else if ((current.Location.Y - parent.Location.Y) == 1)
                {
                    symbol = 'v';
                }
                else if ((current.Location.Y - parent.Location.Y) == -1)
                {
                    symbol = '^';
                }

                if (CharMap[current.Location.X, current.Location.Y] != symbol)
                {
                    CharMap[current.Location.X, current.Location.Y] = symbol;
                    (int x, int y, char @char) foo = new(current.Location.X, current.Location.Y, symbol);
                    DrawQueue.Enqueue(foo);
                }
                path.Add(new (current.Location, symbol));
                current = parent;
                parent = parent.ParentNode;
            }

            parent = current.ParentNode;
            current = currentNode;
            while(current != null)
            {
                if(path.Where(x => x.Item1.X == current.Location.X && x.Item1.Y == current.Location.Y).Any())
                {
                    current = current.ParentNode;
                    continue;
                }
                if (CharMap[current.Location.X, current.Location.Y] != ' ')
                {
                    CharMap[current.Location.X, current.Location.Y] = ' ';
                    (int x, int y, char @char) foo = new(current.Location.X, current.Location.Y, ' ');
                    DrawQueue.Enqueue(foo);
                }
                current = current.ParentNode;
            }
        }

        private void DrawPath(Node currentNode)
        {
            var current = currentNode;
            var parent = currentNode.ParentNode;
            while (parent != null)
            {
                Console.SetCursorPosition(parent.Location.X, parent.Location.Y);
                char symbol = '.';
                if ((current.Location.X - parent.Location.X) == 1)
                {
                    symbol = '>';
                }
                else if ((current.Location.X - parent.Location.X) == -1)
                {
                    symbol = '<';
                }
                else if ((current.Location.Y - parent.Location.Y) == 1)
                {
                    symbol = 'v';
                }
                else if ((current.Location.Y - parent.Location.Y) == -1)
                {
                    symbol = '^';
                }

                if (CharMap[current.Location.X, current.Location.Y] != symbol)
                {
                    CharMap[current.Location.X, current.Location.Y] = symbol;
                    (int x, int y, char @char) foo = new(current.Location.X, current.Location.Y, symbol);
                    DrawQueue.Enqueue(foo);
                }
                //Console.ForegroundColor = ConsoleColor.Green;
                //Console.Write(symbol);
                current = parent;
                parent = parent.ParentNode;
            }
        }

        private void DrawOpenCell(Node currentNode, Node nextNode)
        {
            //Console.SetCursorPosition(currentNode.Location.X, currentNode.Location.Y);
            //Console.Write(" ");

            if (CharMap[currentNode.Location.X, currentNode.Location.Y] != ' ')
            {
                CharMap[currentNode.Location.X, currentNode.Location.Y] = ' ';
                (int y, int x, char @char) foo = new(currentNode.Location.X, currentNode.Location.Y, ' ');
                DrawQueue.Enqueue(foo);
            }

            var oldParent = currentNode.ParentNode;
            List<Node> parents = new();
            while (oldParent != null)
            {
                parents.Add(oldParent);
                if (CharMap[oldParent.Location.X, oldParent.Location.Y] != ' ')
                {
                    CharMap[oldParent.Location.X, oldParent.Location.Y] = ' ';
                    (int y, int x, char @char) foo = new(oldParent.Location.X, oldParent.Location.Y, ' ');
                    DrawQueue.Enqueue(foo);
                }
                //Console.SetCursorPosition(oldParent.Location.X, oldParent.Location.Y);
                //Console.Write(" ");
                oldParent = oldParent.ParentNode;
            }

            oldParent = nextNode.ParentNode;
            List<Node> parents2 = new();
            while (oldParent != null)
            {
                parents.Add(oldParent);
                if (CharMap[oldParent.Location.X, oldParent.Location.Y] != ' ')
                {
                    CharMap[oldParent.Location.X, oldParent.Location.Y] = ' ';
                    (int y, int x, char @char) foo = new(oldParent.Location.X, oldParent.Location.Y, ' ');
                    DrawQueue.Enqueue(foo);
                }
                //Console.SetCursorPosition(oldParent.Location.X, oldParent.Location.Y);
                //Console.Write(" ");
                oldParent = oldParent.ParentNode;
            }

            oldParent = nextNode.ParentNode;
        }

        private bool Search(Node currentNode, bool shouldDraw)
        {
            currentNode.State = NodeState.Closed;
            GetAdjacentWalkableNodes(currentNode);
            
            while(OpenList.Count > 0)
            {
                if (shouldDraw)
                {
                    Console.SetCursorPosition(currentNode.Location.X, currentNode.Location.Y);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("+");
                }
                var nextNode = OpenList.Dequeue();
                if (nextNode.Location == this.EndNode.Location)
                {
                    return true;
                }
                else
                {
                    if (shouldDraw)
                    {
                        DrawPath(currentNode, nextNode);
                    }

                    currentNode = nextNode;
                    currentNode.State = NodeState.Closed;
                    GetAdjacentWalkableNodes(currentNode);
                }
                while(DrawQueue.Count > 0)
                {
                    var temp = DrawQueue.Dequeue();
                    if(temp.@char == '<' || temp.@char == '>' || temp.@char == 'v' || temp.@char == '^')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.SetCursorPosition(temp.x, temp.y);
                    Console.Write(temp.@char);
                }
            }
            return false;

        }

        public List<Point> GetAdjacentLocations(Point from)
        {
            return new List<Point>()
            {
                new Point(from.X + 1, from.Y, 0),
                new Point(from.X - 1, from.Y, 0),
                new Point(from.X, from.Y + 1, 0),
                new Point(from.X, from.Y - 1, 0)
            };
        }

        private void GetAdjacentWalkableNodes(Node fromNode)
        {
            IEnumerable<Point> nextLocations = GetAdjacentLocations(fromNode.Location);

            foreach (var location in nextLocations)
            {
                int x = location.X;
                int y = location.Y;

                if (x < 0 || x >= this.Width || y < 0 || y >= this.Height)
                    continue;

                Node node = Nodes[y][x];
                if (!node.IsWalkable(fromNode))
                    continue;

                if (node.State == NodeState.Closed)
                    continue;

                if (node.State == NodeState.Open)
                {
                    //float traversalCost = Node.GetTraversalCost(node.Location, node.ParentNode.Location);
                    int traversalCost = 1;
                    int gTemp = fromNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.ParentNode = fromNode;
                        node.G = gTemp;
                        var items = new PriorityQueue<Node, double>(OpenList.UnorderedItems);
                        OpenList.Clear();
                        OpenList.EnqueueRange(items.UnorderedItems.Where(x => x.Element != node));
                        OpenList.Enqueue(node, node.F);
                    }
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    node.ParentNode = fromNode;
                    node.State = NodeState.Open;
                    if (ShouldDraw)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(node.Location.X, node.Location.Y);
                        Console.Write('x');
                    }
                    node.G = fromNode.G + 1;
                    OpenList.Enqueue(node, node.F);
                }
            }
        }
    }

    internal class Node
    {
        public Node(Point location, Point endLocation)
        {
            Location = location;
            ParentNode = null;

            if (endLocation != null)
            {
                var heightDiff = endLocation.Height - location.Height;
                var manhattanDistance = Math.Abs(location.X - endLocation.X) + Math.Abs(location.Y - endLocation.Y);

                H = manhattanDistance + heightDiff * 100;
                H = Math.Max(manhattanDistance, heightDiff);
            }
            State = NodeState.Untested;
        }

        public Point Location { get; private set; }
        public bool IsWalkable(Node fromNode)
        {
            if (fromNode.Location.Height + 1 >= this.Location.Height)
            {
                return true;
            }
            return false;
        }

        // G: The length of the path from the start node to this node.
        public int G { get; set; }
        // H: The distance from this node to the end node.
        public double H { get; set; }
        // F: Estimated total distance/cost.
        public double F { get { return this.G + this.H; } }
        public NodeState State { get; set; }
        public Node ParentNode { get; set; }
    }

    public enum NodeState { Untested, Open, Closed }
}
