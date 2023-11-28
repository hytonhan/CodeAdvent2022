using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            var solution = Calculate(startValve, 30, 0, allOpenValves, distanceMatrix);
            Console.WriteLine($"solution calc: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();

            Console.WriteLine($"Best pressure: {solution}");
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
