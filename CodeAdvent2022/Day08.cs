using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    internal class Day08 : IDay
    {
        public int Order => 8;

        const string _inputFilename = "day08_input.txt";

        public void Run()
        {
            string[] lines = File.ReadAllLines(_inputFilename);

            int[,] data = new int[99, 99];
            bool[,] visible = new bool[97, 97];

            for (int i = 0; i < 97; i++)
            {
                for (int j = 0; j < 97; j++)
                {
                    visible[i, j] = false;
                }
            }
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (int j = 0; j < 99; j++)
                {
                    char @char = line[j];
                    data[i, j] = int.Parse(@char.ToString());
                }
            }

            for (int i = 1; i < 98; i++)
            {
                int lastVisibleInRowFromLeft = data[i, 0];
                int lastVisibleInColumnFromTop = data[0, i];

                int lastVisibleInRowFromRight = data[i, 98];
                int lastVisibleInColumnFromBottom = data[98, i];
                for (int j = 1; j < 98; j++)
                {
                    int currentInRowFromLeft = data[i, j];
                    if (currentInRowFromLeft > lastVisibleInRowFromLeft)
                    {
                        visible[i - 1, j - 1] = true;
                        lastVisibleInRowFromLeft = currentInRowFromLeft;
                    }

                    int currentInColumnFromTop = data[j, i];
                    if (currentInColumnFromTop > lastVisibleInColumnFromTop)
                    {
                        visible[j - 1, i - 1] = true;
                        lastVisibleInColumnFromTop = currentInColumnFromTop;
                    }


                    int currentInRowFromRight = data[i, 99 - j - 1];
                    if (currentInRowFromRight > lastVisibleInRowFromRight)
                    {
                        visible[i - 1, 99 - j - 2] = true;
                        lastVisibleInRowFromRight = currentInRowFromRight;
                    }

                    int currentInColumnFromToBottom = data[99 - j - 1, i];
                    if (currentInColumnFromToBottom > lastVisibleInColumnFromBottom)
                    {
                        visible[99 - j - 2, i - 1] = true;
                        lastVisibleInColumnFromBottom = currentInColumnFromToBottom;
                    }
                }
            }


            int visibleOnEdges = 98 * 4;

            int totalVisible = 0;
            for (int i = 0; i < 97; i++)
            {
                for (int j = 0; j < 97; j++)
                {
                    if (visible[i, j])
                    {
                        totalVisible++;
                    }
                }
            }
            totalVisible += visibleOnEdges;

            Console.WriteLine($"Total visible: {totalVisible}");
        }
    }
}
