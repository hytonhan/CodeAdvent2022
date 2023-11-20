using System.Net.Security;

namespace CodeAdvent2022
{
    public class Day02 : IDay
    {
        public int Order => 2;

        const string _inputFilename = "day02_input.txt";

        enum Play
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }
        enum Outcome
        {
            Lose = 0,
            Draw = 3,
            Win = 6
        }

        public void Run()
        {
            // rock 1, paper 2, scissors 3
            // lose 0, draw 3, win 6

            // a rock, b paper, c sciccors
            //DEFAULT
            // x rock, y paper, z sciccors
            //NEW
            // x lose, y draw, z win
            var opponentMapper = new Dictionary<string, Play>
            {
                { "a", Play.Rock },
                { "b", Play.Paper },
                { "c", Play.Scissors }
            };

            var defaultMapper = new Dictionary<string, Play>
            {
                { "x", Play.Rock },
                { "y", Play.Paper },
                { "z", Play.Scissors }
            };

            var newMapper = new Dictionary<string, Outcome>
            {
                { "x", Outcome.Lose },
                { "y", Outcome.Draw },
                { "z", Outcome.Win }
            };

            string[] lines = File.ReadAllLines(_inputFilename);

            int defaultPoints = CalculatePoints(opponentMapper, defaultMapper, lines);
            int newPoints = CalculatePoints(opponentMapper, newMapper, lines);

            Console.WriteLine($"Points with default strategy: {defaultPoints}");
            Console.WriteLine($"Points with outcome strategy: {newPoints}");
        }

        private static int CalculatePoints(
            Dictionary<string, Play> opponentMapper,
            Dictionary<string, Outcome> meMapper,
            string[] lines)
        {
            int points = 0;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var opponentInput = line[0].ToString().ToLower();
                var meInput = line[2].ToString().ToLower();

                var opponent = opponentMapper[opponentInput];
                var me = meMapper[meInput];

                points += (int)me;
                switch (me)
                {
                    case Outcome.Win:
                        switch (opponent)
                        {
                            case Play.Rock:
                                points += (int)Play.Paper;
                                break;
                            case Play.Paper:
                                points += (int)Play.Scissors;
                                break;
                            case Play.Scissors:
                                points += (int)Play.Rock;
                                break;
                        }
                        break;
                    case Outcome.Draw:
                        points += (int)opponent;
                        break;
                    case Outcome.Lose:
                        switch (opponent)
                        {
                            case Play.Rock:
                                points += (int)Play.Scissors;
                                break;
                            case Play.Paper:
                                points += (int)Play.Rock;
                                break;
                            case Play.Scissors:
                                points += (int)Play.Paper;
                                break;
                        }
                        break;
                }
            }
            return points;
        }

        private static int CalculatePoints(
            Dictionary<string, Play> opponentMapper, 
            Dictionary<string, Play> meMapper, 
            string[] lines)
        {
            int points = 0;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var opponentInput = line[0].ToString().ToLower();
                var meInput = line[2].ToString().ToLower();

                var opponent = opponentMapper[opponentInput];
                var me = meMapper[meInput];

                switch (me)
                {
                    case Play.Rock:
                        points += (int)Play.Rock;
                        switch (opponent)
                        {
                            case Play.Rock:
                                points += (int)Outcome.Draw;
                                break;
                            case Play.Paper:
                                points += (int)Outcome.Lose;
                                break;
                            case Play.Scissors:
                                points += (int)Outcome.Win;
                                break;
                        }
                        break;
                    case Play.Paper:
                        points += (int)Play.Paper;
                        switch (opponent)
                        {
                            case Play.Rock:
                                points += (int)Outcome.Win;
                                break;
                            case Play.Paper:
                                points += (int)Outcome.Draw;
                                break;
                            case Play.Scissors:
                                points += (int)Outcome.Lose;
                                break;
                        }
                        break;
                    case Play.Scissors:
                        points += (int)Play.Scissors;
                        switch (opponent)
                        {
                            case Play.Rock:
                                points += (int)Outcome.Lose;
                                break;
                            case Play.Paper:
                                points += (int)Outcome.Win;
                                break;
                            case Play.Scissors:
                                points += (int)Outcome.Draw;
                                break;
                        }
                        break;
                }
            }

            return points;
        }
    }
}
