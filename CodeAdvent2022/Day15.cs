using System;
using System.Collections.Generic;
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
            int targetY = 10;
            ParseInput(_testInputFilename, out var sensors, out var beacons);
            List<int> coveredBySensorCoords = ProcessSensorCoverageOnTargetRow(sensors, targetY);

            var beaconsOnTargetRow = beacons.Where(x => x.Y == targetY);
            foreach (var beacon in beaconsOnTargetRow)
            {
                coveredBySensorCoords.RemoveAll(x => x == beacon.X);
            }

            List<Line> lines = new();

            List<Line> upLines = new();
            List<Line> downLines = new();
            List<Loc> intersects = new();
            foreach(var sensor in sensors)
            {
                var loc1 = new Loc(sensor.X, sensor.Y + sensor.DistanceToBeacon);
                var loc2 = new Loc(sensor.X + sensor.DistanceToBeacon, sensor.Y);
                var loc3 = new Loc(sensor.X, sensor.Y - sensor.DistanceToBeacon);
                var loc4 = new Loc(sensor.X - sensor.DistanceToBeacon, sensor.Y);
                var line1 = new Line(loc1, loc2, -1);
                var line2 = new Line(loc2, loc3, 1);
                var line3 = new Line(loc3, loc4, -1);
                var line4 = new Line(loc4, loc1, 1);

                lines.Add(line1);
                lines.Add(line2);
                lines.Add(line3);
                lines.Add(line4);

                upLines.Add(line2);
                upLines.Add(line4);

                downLines.Add(line1);
                downLines.Add(line3);
            }

            foreach(var line1 in upLines)
            {
                foreach(var line2 in downLines)
                {
                    if (Intersects(line1, line2, out Loc intersect))
                    {
                        intersects.Add(intersect);
                    }
                }
            }

            var filtered = intersects.Where(x => x.X >= 0 && x.X <= 20 && x.Y >= 0 && x.Y <= 20).ToList(); ;

            Console.WriteLine($"Coords where beacon can't be: {coveredBySensorCoords.Distinct().Count()}");
        }

        private bool Intersects(Line line1, Line line2, out Loc intersect)
        {
            intersect = null;
            // Instead of taking x,y as the variables to solve for,
            // write (x,y) =(x1,y1)+t(x2−x1,y2−y1) =(x3,y3)+u(x4−x3,y4−y3) 

            float a1 = line1.End.Y - line1.Start.Y;
            float intercept = line1.Start.Y - line1.Coefficient * line1.Start.X;

            float A1 = line1.End.Y - line1.Start.Y;
            float B1 = line1.Start.X - line1.End.X;
            float C1 = line1.Start.Y * (line1.End.X - line1.Start.X) - (A1) * line1.Start.X;
            float A2 = line2.End.Y - line2.Start.Y;
            float B2 = line2.Start.X - line2.End.X;
            float C2 = line2.Start.Y * (line2.End.X - line2.Start.X) - (A2) * line2.Start.X;

            float delta = A1 * B2 - A2 * B1;
            if (delta == 0)
            {
                Console.WriteLine($"Error!");
                return false;
            }

            float x = (B2 * C1 - B1 * C2) / delta;
            float y = (A1 * C2 - A2 * C1) / delta;

            // DEBUGGERISSA EKAN PITÄIS OLLA INTERSECT pistees (9, 18)
            if (x >= line1.Start.X && x <= line1.End.X)
            {
                intersect = new Loc((int)x, (int)y);
                return true;

            }
            return false;


            //A = y2 - y1; B = x1 - x2; C = Ax1 + By1
            //float A1 = line1.End.Y - line1.Start.Y;
            //float B1 = line1.Start.X - line1.End.X;
            //float A2 = line2.End.Y - line2.Start.Y;
            //float B2 = line2.Start.X - line2.End.X;
            //float C1 = A1 * line2
            //float delta = A1 * B2 - A2 * B1;

            //if (delta == 0)
            //    throw new ArgumentException("Lines are parallel");

            //float x = (B2 * C1 - B1 * C2) / delta;
            //float y = (A1 * C2 - A2 * C1) / delta;
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
        public Loc(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;
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
