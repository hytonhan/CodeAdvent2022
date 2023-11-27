using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    internal class Day15 : IDay
    {
        public int Order => 15;

        const string _inputFilename = "day15_input.txt";
        const string _testInputFilename = "day15_input_test.txt";

        public void Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //int targetY = 10;
            //(int x, int y) dims = new(20, 20);
            int targetY = 2000000;
            (int x, int y) dims = new(4000000, 4000000);

            ParseInput(_inputFilename, out var sensors, out var beacons);
            List<int> coveredBySensorCoords = ProcessSensorCoverageOnTargetRow(sensors, targetY);

            var orderedBeacons = sensors.OrderByDescending(x => x.DistanceToBeacon);

            var beaconsOnTargetRow = beacons.Where(x => x.Y == targetY);
            foreach (var beacon in beaconsOnTargetRow)
            {
                coveredBySensorCoords.RemoveAll(x => x == beacon.X);
            }

            stopwatch.Stop();
            Console.WriteLine($"Part 1 time: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();

            List<Line> upLines = new();
            List<Line> downLines = new();

            foreach (var sensor in sensors)
            {
                var loc1 = new Loc(sensor.X, sensor.Y + sensor.DistanceToBeacon);
                var loc2 = new Loc(sensor.X + sensor.DistanceToBeacon, sensor.Y);
                var loc3 = new Loc(sensor.X, sensor.Y - sensor.DistanceToBeacon);
                var loc4 = new Loc(sensor.X - sensor.DistanceToBeacon, sensor.Y);
                var line1 = new Line(loc1, loc2, -1);
                var line2 = new Line(loc2, loc3, 1);
                var line3 = new Line(loc3, loc4, -1);
                var line4 = new Line(loc4, loc1, 1);

                upLines.Add(line2);
                upLines.Add(line4);

                downLines.Add(line1);
                downLines.Add(line3);
            }

            List<Loc> intersects = new();
            foreach (var line1 in upLines)
            {
                foreach (var line2 in downLines)
                {
                    if (Intersects(line1, line2, out Loc intersect))
                    {
                        if (intersects.Where(x => x.X == intersect.X && x.Y == intersect.Y).Any())
                        {
                            continue;
                        }
                        intersects.Add(intersect);
                    }
                }
            }

            List<Loc> places = new();
            foreach (var intersect in intersects)
            {
                var right = new Loc(intersect.X + 1, intersect.Y);
                var left = new Loc(intersect.X - 1, intersect.Y);
                var top = new Loc(intersect.X, intersect.Y + 1);
                var bottom = new Loc(intersect.X, intersect.Y - 1);

                var temp = new List<Loc>()
                {
                    right,
                    left,
                    top,
                    bottom
                };
                foreach (var location in temp)
                {
                    var anySensorInRage = sensors.Where(sensor => Math.Abs(location.X - sensor.X) + Math.Abs(location.Y - sensor.Y) <= sensor.DistanceToBeacon).Any();
                    if (anySensorInRage == false)
                    {
                        if (location.X < 0 || location.X > dims.x || location.Y < 0 || location.Y > dims.y)
                        {
                            continue;
                        }
                        if (places.Where(x => x.X == location.X && x.Y == location.Y).Any())
                        {
                            continue;
                        }
                        places.Add(location);
                    }
                }
            }
            Console.WriteLine($"Part 2 time: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Stop();

            Console.WriteLine($"Coords where beacon can't be: {coveredBySensorCoords.Distinct().Count()}");

            var places2 = places.Where(x => x.X >= 0 && x.Y >= 0 && x.X <= dims.x && x.Y <= dims.y);
            var winner = places2.Single();
            Console.WriteLine($"Tuning freq: {winner.X * 4000000 + winner.Y}");
        }

        private bool Intersects(Line line1, Line line2, out Loc intersect)
        {
            intersect = null;
            long A1 = line1.End.Y - line1.Start.Y;
            long B1 = line1.Start.X - line1.End.X;
            long C1 = line1.Start.Y * (line1.End.X - line1.Start.X) - (A1) * line1.Start.X;
            long A2 = line2.End.Y - line2.Start.Y;
            long B2 = line2.Start.X - line2.End.X;
            long C2 = line2.Start.Y * (line2.End.X - line2.Start.X) - (A2) * line2.Start.X;

            long determinant = A1 * B2 - A2 * B1;

            long x = (B2 * -C1 - B1 * -C2) / determinant;
            long y = (A1 * -C2 - A2 * -C1) / determinant;

            if (x >= line1.Start.X && x <= line1.End.X ||
                x >= line1.End.X && x <= line1.Start.X)
            {
                intersect = new Loc((int)x, (int)y);
                return true;

            }
            return false;
        }

        private List<int> ProcessSensorCoverageOnTargetRow(
            List<Sensor> sensors,
            int targetY)
        {
            List<int> coveredBySensorCoords = new();
            foreach (var sensor in sensors)
            {
                var distanceToTargetY = Math.Abs(sensor.Y - targetY);
                var sensorX = sensor.X;

                if (distanceToTargetY > sensor.DistanceToBeacon)
                {
                    continue;
                }
                var coverageOnTargetRow = sensor.DistanceToBeacon - distanceToTargetY + 1;

                for (int i = 0; i < coverageOnTargetRow; i++)
                {
                    var minusX = sensorX - i;
                    coveredBySensorCoords.Add(minusX);
                    var positiveX = sensorX + i;
                    coveredBySensorCoords.Add(positiveX);
                }
            }

            return coveredBySensorCoords.Distinct().ToList();
        }

        private void ParseInput(string filename,
                                out List<Sensor> sensors,
                                out List<Beacon> beacons)
        {
            sensors = new();
            beacons = new();

            string[] lines = File.ReadAllLines(filename);
            foreach (var line in lines)
            {
                string coords = line.Replace("Sensor at ", "")
                                    .Replace(" closest beacon is at ", "")
                                    .Replace(" ", "")
                                    .Replace("x=", "")
                                    .Replace("y=", "");
                var parts = coords.Split(':');
                var sensorData = parts[0].Split(',');
                var beaconData = parts[1].Split(',');

                var beacon = new Beacon(int.Parse(beaconData[0]), int.Parse(beaconData[1]));
                beacons.Add(beacon);
                var sensor = new Sensor(int.Parse(sensorData[0]), int.Parse(sensorData[1]), beacon);
                sensors.Add(sensor);
            }
        }
    }

    class Line
    {
        public Line(Loc start, Loc end, int coefficient)
        {
            Start = start;
            End = end;
            Coefficient = coefficient;
        }

        public Line(int startx, int starty, int endx, int endy)
        {
            Start = new Loc(startx, starty);
            End = new Loc(endx, endy);
        }

        public Loc Start { get; set; }
        public Loc End { get; set; }

        public int Coefficient { get; set; }
    }

    class Loc
    {
        public Loc(long x, long y)
        {
            X = x;
            Y = y;
        }

        public long X;
        public long Y;
    }

    class Beacon
    {
        public Beacon(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
    }

    class Sensor
    {
        public Sensor(int x, int y, Beacon beacon)
        {
            X = x;
            Y = y;
            ClosestBeacon = beacon;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public Beacon ClosestBeacon { get; set; }

        public int DistanceToBeacon => Math.Abs(ClosestBeacon.X - X) + Math.Abs(ClosestBeacon.Y - Y);
    }
}
