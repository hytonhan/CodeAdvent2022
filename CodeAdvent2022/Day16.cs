using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    internal class Day16 : IDay
    {
        public int Order => 16;

        const string _inputFilename = "day16_input.txt";
        const string _testInputFilename = "day16_input_test.txt";

        private Dictionary<string, Dictionary<string, int>> _matrix2 = new();

        public void Run()
        {
            List<Valve> valves = ParseInput(_inputFilename);
            InitDistanceMatrix(valves);
            Valve startValve = valves.Where(x => x.Id == "AA").First();
            Dictionary<Valve, bool> allOpenValves = new Dictionary<Valve, bool>(valves.Where(x => x.FlowRate != 0).Select(x => new KeyValuePair<Valve, bool>(x, false)));

            var solution = Calculate(startValve, 30, 0, allOpenValves);
            var solution2 = Calculate(startValve, 30, 0, allOpenValves, _matrix3);

            Console.WriteLine($"Best pressure: {solution}");
        }

        private void InitDistanceMatrix(List<Valve> valves)
        {
            foreach (var valve in valves)
            {
                Dictionary<string, int> temp2 = new();
                var otherValves = valves.Where(x => x != valve).ToList();
                foreach (var other in otherValves)
                {
                    var start = valve;
                    var destination = other;

                    int distance = 0;
                    List<Valve> nextValveList = new();
                    nextValveList.Add(start);
                    bool endFound = false;
                    while (true)
                    {
                        distance++;
                        List<Valve> tempList = new();
                        foreach (var nextValve in nextValveList)
                        {
                            endFound = Foo(nextValve, destination);
                            if (endFound) break;
                            tempList.AddRange(nextValve.LeadsTo);
                        }

                        if (endFound) break;
                        nextValveList.Clear();
                        nextValveList.AddRange(tempList);
                    }
                    temp2.Add(destination.Id, distance);
                }
                _matrix2.Add(valve.Id, temp2);
            }
        }

        private int Calculate(Valve start, int time, int score, Dictionary<Valve, bool> openValves)
        {
            var timeBackup = time;
            var scoreBackup = score;
            var scores = new List<int>();
            foreach (var openValve in openValves)
            {
                var newValves = new Dictionary<Valve, bool>(openValves.Where(x => x.Key != openValve.Key));
                var distance = _matrix2[start.Id][openValve.Key.Id];
                if (distance + 1 > time)
                {
                    scores.Add(score);
                }
                else
                {
                    time -= distance + 1;
                    score += time * openValve.Key.FlowRate;
                    scores.Add(Calculate(openValve.Key, time, score, newValves));
                }
                time = timeBackup;
                score = scoreBackup;
            }
            var returnValue = scores.Count == 0 ? score : scores.Max();
            return returnValue;
        }

        private bool Foo(Valve start, Valve destination)
        {
            foreach (var valve in start.LeadsTo)
            {
                if (valve == destination) return true;
            }
            return false;
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
