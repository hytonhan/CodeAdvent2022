using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    internal class Day12 : IDay
    {
        public int Order => 12;
        const string _inputFilename = "day12_input.txt";
        const string _testInputFilename = "day12_input_test.txt";

        public void Run()
        {
            string[] lines = File.ReadAllLines(_testInputFilename);

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
                        //points.Add(start);
                    }
                    else if (lines[i][j] == 'E')
                    {
                        end = new Point(j, i, 'z' - 'a');
                        //points.Add(end);
                    }
                    else
                    {
                        var temp = new Point(j, i, lines[i][j] - 'a');
                        points.Add(temp);
                    }
                }
            }

            //Node startNode = new Node(start!, null);
            //Node endNode = new Node(end, null);

            var search = new SearchClass(points, start, end, width, height);
            
            var path = search.FindPath();
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

    internal class SearchParams
    {
        public Point Start { get; set; }
        public Point End { get; set; }
        public bool[,] Map { get; set; }
    }

    internal class SearchClass
    {
        public SearchClass(List<Point> points, Point start, Point end, int width, int height)
        {
            StartNode = new Node(start, null);
            EndNode = new Node(end, null);
            Nodes = new List<Node>();
            Nodes.Add(StartNode);
            Nodes.Add(EndNode);
            foreach (Point p in points)
            {
                Nodes.Add(new Node(p, end));
            }
            Width = width;
            Height = height;
        }

        public Node StartNode { get; set; }
        public Node EndNode { get; set; }

        public List<Node> Nodes { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }


        public List<Point> FindPath()
        {
            List<Point> path = new List<Point>();
            bool success = Search(StartNode);
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
            return path;
        }

        private bool Search(Node currentNode)
        {
            currentNode.State = NodeState.Closed;
            List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);
            nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            foreach (Node nextNode in nextNodes)
            {
                if (nextNode.Location == this.EndNode.Location)
                {
                    return true;
                }
                else
                {
                    if (Search(nextNode)) // Note: Recurses back into Search(Node)
                        return true;
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
                new Point(from.X, from.Y + 1, 0)
            };
        }

        private List<Node> GetAdjacentWalkableNodes(Node fromNode)
        {
            List<Node> walkableNodes = new List<Node>();
            IEnumerable<Point> nextLocations = GetAdjacentLocations(fromNode.Location);

            foreach (var location in nextLocations)
            {
                int x = location.X;
                int y = location.Y;

                // Stay within the grid's boundaries
                if (x < 0 || x >= this.Width || y < 0 || y >= this.Height)
                    continue;

                Node node = this.Nodes.Where(node => node.Location.X == x && node.Location.Y == y).Single();
                // Ignore non-walkable nodes
                if (!node.IsWalkable(fromNode))
                    continue;

                // Ignore already-closed nodes
                if (node.State == NodeState.Closed)
                    continue;

                // Already-open nodes are only added to the list if their G-value is lower going via this route.
                if (node.State == NodeState.Open)
                {
                    //float traversalCost = Node.GetTraversalCost(node.Location, node.ParentNode.Location);
                    int traversalCost = 1;
                    int gTemp = fromNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.ParentNode = fromNode;
                        node.G = gTemp;
                        walkableNodes.Add(node);
                    }
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    node.ParentNode = fromNode;
                    node.State = NodeState.Open;
                    node.G = fromNode.G + 1;
                    walkableNodes.Add(node);
                }
            }

            return walkableNodes;
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
        public int H { get; private set; }
        // F: Estimated total distance/cost.
        public int F { get { return this.G + this.H; } }
        public NodeState State { get; set; }
        public Node ParentNode { get; set; }
    }

    public enum NodeState { Untested, Open, Closed }
}
