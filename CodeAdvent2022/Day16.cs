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
            List<Valve> valves = ParseInput(_inputFilename);

            Dictionary<string, bool> visited = InitVisitedList(valves);
            var distanceMatrix = InitDistanceMatrix(valves, visited);

            Valve startValve = valves.Where(x => x.Id == "AA").First();
            var allOpenValves = new Dictionary<Valve, bool>(valves.Where(x => x.FlowRate != 0)
                                                                  .Select(x => new KeyValuePair<Valve, bool>(x, false)));
            Dictionary<int, int> maxScores = new()
            {
                { 24, 0 },
                { 19, 0 },
                { 14, 0 },
                { 9, 0 },
                { 4, 0 },
            };
            var solution = Part1v2(startValve, 30, 0, 0, 0, allOpenValves, distanceMatrix, ref maxScores);

            Dictionary<int, int> maxScores2 = new()
            {
                { 19, 0 },
                { 14, 0 },
                { 9, 0 },
                { 4, 0 },
            };
            var solution2 = Part2v2(startValve, startValve, 26, 0, 0, 0, 0, 0, allOpenValves, distanceMatrix, ref maxScores2);

            Console.WriteLine($"Best pressure: {solution}");
            Console.WriteLine($"Best pressure with 1 elephant: {solution2}");
        }
        private Dictionary<string, Dictionary<string, int>> InitDistanceMatrix(
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

        private int Part2v2(
            Valve start,
            Valve start2,
            int time,
            int score,
            int pressure,
            int pressure2,
            int distanceToNext,
            int distanceToNext2,
            Dictionary<Valve, bool> openValves,
            Dictionary<string, Dictionary<string, int>> distanceMatrix,
            ref Dictionary<int, int> maxScores)
        {
            while (distanceToNext > 0 && distanceToNext2 > 0)
            {
                score += pressure + pressure2;
                distanceToNext--;
                distanceToNext2--;
                time--;
                // Keep track of scores, and return if seems like current path is doomed
                if (time == 19 || time == 14 || time == 9 || time == 4)
                {
                    if (maxScores[time] > score)
                    {
                        return 0;
                    }
                    else if (score > maxScores[time])
                    {
                        maxScores[time] = score;
                    }
                }
                if (time == 0)
                {
                    return score;
                }
            }
            var pathScores = new List<int>();

            if (distanceToNext == 0)
            {
                pressure += start.FlowRate;

                foreach (var openValve in openValves)
                {
                    var newValves = new Dictionary<Valve, bool>(openValves.Where(x => x.Key != openValve.Key));

                    distanceToNext = distanceMatrix[start.Id][openValve.Key.Id] + 1;

                    pathScores.Add(Part2v2(openValve.Key, start2, time, score, pressure, pressure2, distanceToNext, distanceToNext2, newValves, distanceMatrix, ref maxScores));
                }
            }
            if(distanceToNext2 == 0)
            {
                pressure2 += start2.FlowRate;

                foreach (var openValve in openValves)
                {
                    var newValves = new Dictionary<Valve, bool>(openValves.Where(x => x.Key != openValve.Key));

                    distanceToNext2 = distanceMatrix[start2.Id][openValve.Key.Id] + 1;

                    pathScores.Add(Part2v2(start, openValve.Key, time, score, pressure, pressure2, distanceToNext, distanceToNext2, newValves, distanceMatrix, ref maxScores));
                }
            }

            return pathScores.Count() > 0 ? pathScores.Max() : 0;
        }

        private int Part1v2(
            Valve start,
            int time,
            int score,
            int pressure,
            int distanceToNext,
            Dictionary<Valve, bool> openValves,
            Dictionary<string, Dictionary<string, int>> distanceMatrix,
            ref Dictionary<int, int> maxScores)
        {
            while (distanceToNext > 0)
            {
                score += pressure;
                distanceToNext--;
                time--;
                // Keep track of scores, and return if seems like current path is doomed
                if (time == 24 || time == 19 || time == 14 || time == 9 || time == 4)
                {
                    if (maxScores[time] > score)
                    {
                        return 0;
                    }
                    else if (score > maxScores[time])
                    {
                        maxScores[time] = score;
                    }
                }
                if (time == 0)
                {
                    return score;
                }
            }
            pressure += start.FlowRate;

            var pathScores = new List<int>();
            foreach (var openValve in openValves)
            {
                var newValves = new Dictionary<Valve, bool>(openValves.Where(x => x.Key != openValve.Key));

                distanceToNext = distanceMatrix[start.Id][openValve.Key.Id] + 1;

                pathScores.Add(Part1v2(openValve.Key, time, score, pressure, distanceToNext, newValves, distanceMatrix, ref maxScores));
            }
            return pathScores.Count() > 0 ? pathScores.Max() : 0;
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
