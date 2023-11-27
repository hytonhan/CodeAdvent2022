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

        public void Run()
        {
            List<Valve> valves = ParseInput(_testInputFilename);

            Dictionary<Valve, Dictionary<Valve, int>> matrix = new();

            foreach(var valve in valves)
            {
                Dictionary<Valve, int> temp = new();
                var otherValves = valves.Where(x => x != valve).ToList();
                foreach(var other in otherValves)
                {
                    var start = valve;
                    var destination = other;

                    int distance = 0;
                    while(Foo(start, destination, ref distance) == false)
                    {

                    }

                    //Find shortest path to other
                }
            }
        }

        private bool Foo(Valve start, Valve destination, ref int distance)
        {
            distance++;
            foreach(var valve in start.LeadsTo)
            {
                if (valve == destination) return true;
            }
            return false;
        }

        private List<Valve> ParseInput(string filename)
        {
            List<Valve> valves = new();

            string[] lines = File.ReadAllLines(_testInputFilename);
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
