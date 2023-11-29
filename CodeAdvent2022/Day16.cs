using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeAdvent2022
{
    internal class Day16 : IDay
    {
        public int Order => 16;

        const string _inputFilename = "day16_input.txt";
        const string _testInputFilename = "day16_input_test.txt";

        private Queue<Valve> _openList = new();

        public void Run()
        {
            _openList.Clear();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Valve> valves = ParseInput(_inputFilename);
            Console.WriteLine($"Input time: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();

            Dictionary<string, bool> visited = InitVisitedList(valves);
            var distanceMatrix = InitDistanceMatrix2(valves, visited);
            Console.WriteLine($"Matrix init: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();

            Valve startValve = valves.Where(x => x.Id == "AA").First();
            var allOpenValves = new Dictionary<Valve, bool>(valves.Where(x => x.FlowRate != 0)
                                                                  .Select(x => new KeyValuePair<Valve, bool>(x, false)));
            //var solution = Calculate(startValve, 30, 0, allOpenValves, distanceMatrix);

            //var sol2 = Calculat2(startValve, 26, 0, allOpenValves, distanceMatrix);

            List<string> output = new();
            List<int> scores = new();
            Calculate3(startValve, 26, 0, allOpenValves, distanceMatrix, ref output, ref scores, "");

            var maxPathLength = output.Max(x => x.Split(',').Length);
            var minPathLength = output.Min(x => x.Split(',').Length);

            List<string> paths = new();
            //AnotherOne(startValve, allOpenValves, 0, 8, ref paths, "");

            var pathsOf8 = output.Where(x => x.Split(',').Length == 8).ToList();
            var pathsOf7 = output.Where(x => x.Split(',').Length == 7).ToList();
            var pathsOf6 = output.Where(x => x.Split(',').Length == 6).ToList();
            //var pathsOf5 = output.Where(x => x.Split(',').Length == 5).ToList();
            //var pathsOf4 = output.Where(x => x.Split(',').Length == 4).ToList();

            List<string> test = new();
            test.AddRange(pathsOf8);
            test.AddRange(pathsOf7);
            test.AddRange(pathsOf6);

            var bar = output.Select(x => x.Split(',')[0]).Distinct().ToList();

            var ones = output.Where(x => x.Split(',').Length < 2);

            int max = 0;

            var pos = Console.GetCursorPosition();
            for (int i = 0; i < test.Count(); i++)
            {
                Console.SetCursorPosition(pos.Left, pos.Top);
                Console.WriteLine($"{i}/{test.Count()} processed");

                var path = output[i];
                var score = scores[output.IndexOf(path)];

                var pathNodes = path.Split(',');

                //var others = output.Where(x => !pathNodes.Any(y => output.Contains(y))).ToList();

                //List<string> paths2 = new(output);
                //foreach(var pathNode in pathNodes)
                //{
                //    paths2 = paths2.Where(x => !x.Contains(pathNode)).ToList();
                //}

                foreach (var elephantPath in test)
                {
                    if (elephantPath.Split(',').ToList().Any(x => pathNodes.Contains(x)))
                    {
                        continue;
                    }
                    var parts2 = elephantPath.Split(',');
                    bool skipPath = false;
                    foreach (var part in parts2)
                    {
                        if (path.Contains(part))
                        {
                            skipPath = true;
                            break;
                        }
                    }
                    if (skipPath)
                    {
                        continue;
                    }
                    if (score + scores[output.IndexOf(elephantPath)] > max)
                    {
                        max = score + scores[output.IndexOf(elephantPath)];
                    }
                }

            }

            //List<string> output2 = new();
            //List<int> scores2 = new();
            //Calculate3(startValve, 30, 0, allOpenValves, distanceMatrix, ref output2, ref scores2, "");
            //var max = scores2.Max();

            Console.WriteLine($"solution calc: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();

            //Console.WriteLine($"Best pressure: {solution}");
            Console.WriteLine($"Best pressure with elephant: {max}");
        }

        private Dictionary<string, Dictionary<string, int>> InitDistanceMatrix2(
            List<Valve> valves,
            Dictionary<string, bool> visited)
        {
            var returnValue = new Dictionary<string, Dictionary<string, int>>();
            foreach (var valve in valves)
            {
                var otherValves = valves.Where(x => x != valve).ToList();
                var temp = new Dictionary<string, int>();
                foreach (var other in otherValves)
                {
                    foreach (var key in visited)
                    {
                        visited[key.Key] = false;
                    }
                    int distance = 0;
                    int finalDistance = 0;
                    CalcDistanceToValve(valve, other, ref visited, distance, ref finalDistance);
                    temp.Add(other.Id, finalDistance);
                }
                returnValue.Add(valve.Id, temp);
            }
            return returnValue;
        }

        private Dictionary<string, bool> InitVisitedList(List<Valve> valves)
        {
            var returnValue = new Dictionary<string, bool>();
            foreach (var valve in valves)
            {
                returnValue.Add(valve.Id, false);
            }
            return returnValue;
        }

        private void CalcDistanceToValve(
            Valve start,
            Valve destination,
            ref Dictionary<string, bool> visited,
            int distance,
            ref int finalDist)
        {
            finalDist = distance;
            visited[start.Id] = true;
            bool found = false;

            if (start.Id == destination.Id)
            {
                return;
            }

            Queue<Valve> queue = new();
            foreach (var child in start.LeadsTo)
            {
                queue.Enqueue(child);
            }

            while (found == false)
            {
                distance++;
                var next = new List<Valve>();
                while (queue.Count > 0)
                {
                    var node = queue.Dequeue();
                    visited[node.Id] = true;
                    if (node.Id == destination.Id)
                    {
                        found = true;
                        break;
                    }
                    next.AddRange(node.LeadsTo);
                }
                foreach (var foo in next)
                {
                    if (visited[foo.Id])
                    {
                        continue;
                    }
                    queue.Enqueue(foo);
                }
            }
            finalDist = distance;
            return;
        }

        private void AnotherOne(
            Valve start,
            Dictionary<Valve, bool> openValves,
            int depth,
            int maxDepth,
            ref List<string> _out,
            string path)
        {
            depth++;
            var pathBackup = path;

            foreach (var openValve in openValves)
            {
                if (path.Length == 0)
                {
                    path = openValve.Key.Id;
                }
                else
                {
                    path += "," + openValve.Key.Id;
                }
                if (path.Split(',').Length >= maxDepth)
                {
                    _out.Add(path);
                    path = pathBackup;
                    continue;
                }

                var newValves = new Dictionary<Valve, bool>(openValves.Where(x => x.Key != openValve.Key));
                AnotherOne(openValve.Key, newValves, depth, maxDepth, ref _out, path);

                path = pathBackup;
            }
        }

        private void Calculate3(
            Valve start,
            int time,
            int score,
            Dictionary<Valve, bool> openValves,
            Dictionary<string, Dictionary<string, int>> distanceMatrix,
            ref List<string> _output,
            ref List<int> _pathScores,
            string parents)
        {
            var timeBackup = time;
            var scoreBackup = score;
            var parentsBackup = parents;
            var list = new List<Valve>();
            bool thisAdded = false;
            foreach (var openValve in openValves)
            {
                var newValves = new Dictionary<Valve, bool>(openValves.Where(x => x.Key != openValve.Key));
                var distance = distanceMatrix[start.Id][openValve.Key.Id];
                if (distance + 1 > time)
                {
                    if (thisAdded)
                    {
                        continue;
                    }
                    if (parents.Split(',').Length > 8)
                    {
                        var relevant = parents.Substring(23);
                        if (_output.Contains(relevant) == false)
                        {
                            _output.Add(relevant);
                            thisAdded = true;
                            _pathScores.Add(score);
                        }
                    }
                    //if (parents.Split(',').Length != 8 && parents.Split(',').Length != 7)
                    //{
                    //    continue;
                    //}
                    _output.Add(parents);
                    thisAdded = true;
                    _pathScores.Add(score);

                }
                else
                {
                    if (parents.Length == 0)
                    {
                        parents = openValve.Key.Id;
                    }
                    //parents.Add(openValve.Key.Id);
                    else
                    {
                        parents += "," + openValve.Key.Id;
                    }
                    time -= distance + 1;
                    score += time * openValve.Key.FlowRate;
                    Calculate3(openValve.Key, time, score, newValves, distanceMatrix, ref _output, ref _pathScores, parents);
                }
                time = timeBackup;
                score = scoreBackup;
                parents = parentsBackup;
            }

        }

        //private List<Valve> Calculat2(
        //    Valve start,
        //    int time,
        //    int score,
        //    Dictionary<Valve, bool> openValves,
        //    Dictionary<string, Dictionary<string, int>> distanceMatrix,
        //    Foo parent)
        //{

        //long coeffFor8 = BinomialCoefficient(15, 8);
        //long coeffFor7 = BinomialCoefficient(15, 7);

        //var paths = new Dictionary<string, List<string>>();

        //var list = new List<Valve>();

        //foreach (var openValve in openValves)
        //{
        //    var foo = new Foo(openValve.Key);
        //    list.Add(foo);

        //    var newValves = new Dictionary<Valve, bool>(openValves.Where(x => x.Key != openValve.Key));
        //    var distance = distanceMatrix[start.Id][openValve.Key.Id];
        //    if (distance + 1 > time)
        //    {
        //    }
        //    else
        //    {
        //        if (parent != null)
        //        {
        //            parent.Children.Add(list);
        //        }
        //        time -= distance + 1;
        //        score += time * openValve.Key.FlowRate;
        //        var children = Calculat2(openValve.Key, time, score, newValves, distanceMatrix, foo);
        //        foreach (var child in children)
        //        {
        //            list.Add(child);
        //        }
        //    }
        //    time = timeBackup;
        //    score = scoreBackup;
        //}

        //return 0;
        //}

        long BinomialCoefficient(int n, int k)
        {
            return Factorial(n) / (Factorial(k) * Factorial(n - k));
        }

        long Factorial(int n)
        {
            if (n == 0 || n == 1)
            {
                return 1;
            }

            return n * Factorial(n - 1);
        }

        private int Calculate(Valve start, int time, int score, Dictionary<Valve, bool> openValves, Dictionary<string, Dictionary<string, int>> distanceMatrix)
        {
            var timeBackup = time;
            var scoreBackup = score;
            var scores = new List<int>();
            foreach (var openValve in openValves)
            {
                var newValves = new Dictionary<Valve, bool>(openValves.Where(x => x.Key != openValve.Key));
                var distance = distanceMatrix[start.Id][openValve.Key.Id];
                if (distance + 1 > time)
                {
                    scores.Add(score);
                }
                else
                {
                    time -= distance + 1;
                    score += time * openValve.Key.FlowRate;
                    scores.Add(Calculate(openValve.Key, time, score, newValves, distanceMatrix));
                }
                time = timeBackup;
                score = scoreBackup;
            }
            var returnValue = scores.Count == 0 ? score : scores.Max();
            return returnValue;
        }

        private List<Valve> ParseInput(string filename)
        {
            List<Valve> valves = new();

            string[] lines = File.ReadAllLines(filename);
            foreach (var line in lines)
            {
                var parts = line.Replace("Valve ", "")
                                .Replace(" has flow rate=", ";")
                                .Replace("tunnels", "tunnel")
                                .Replace("leads", "lead")
                                .Replace("valves", "valve")
                                .Replace("; tunnel lead to valve ", ";")
                                .Split(";");
                var id = parts[0];
                var rate = int.Parse(parts[1]);
                var leadTo = parts[2].Replace(" ", "").Split(",");
                var valve = new Valve(id, rate, leadTo.ToList());
                valves.Add(valve);
            }
            foreach (var valve in valves)
            {
                foreach (var otherValveId in valve.LeadsToInit)
                {
                    var otherValve = valves.Where(x => x.Id == otherValveId).Single();
                    valve.LeadsTo.Add(otherValve);
                }
            }

            return valves;
        }
    }

    class Foo
    {
        public Foo(Valve valve)
        {
            Valve = valve;
            Children = new();
        }

        public Valve Valve { get; set; }

        public List<Foo> Children { get; set; }
    }
    class Valve
    {
        public Valve(string id, int flow, List<string> leadsTo)
        {
            Id = id;
            FlowRate = flow;
            LeadsToInit = leadsTo;
            LeadsTo = new();
        }

        public string Id { get; set; }

        public int FlowRate { get; set; }

        public List<string> LeadsToInit { get; set; }

        public List<Valve> LeadsTo { get; set; }
    }
}
