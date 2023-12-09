using System.Diagnostics.Metrics;
using System.Globalization;

namespace day8;

class Program
{
    static void Main(string[] args)
    {
        StreamReader sr = new("input.txt");
        string? line;
        int lineNumber = 0;
        List<string> directions = [];

        Dictionary<string, Tuple<string, string>> map = [];

        while ((line = sr.ReadLine()) != null)
        {
            if (lineNumber == 0)
            {
                directions = line.Select(x => x.ToString()).ToList();
            }
            else if (line.Length > 0)
            {
                // not blank line - let's parse it
                string[] splitted = line.Split(" = ");
                string[] items = splitted[1].Replace("(", "").Replace(")", "").Split(", ");

                map[splitted[0]] = new Tuple<string, string>(items[0], items[1]);
            }
            lineNumber++;
        }

        // q1
        int q1 = 0;
        int indexDirection = 0;
        string location = "AAA";

        while (location != "ZZZ")
        {
            Console.WriteLine($"Currently at {location}. Going {directions[indexDirection]}");

            location = directions[indexDirection] switch
            {
                "L" => map[location].Item1,
                "R" => map[location].Item2,
                _ => throw new ArgumentException($"Invalid direction {directions[indexDirection]}")
            };

            q1++;
            if (indexDirection < directions.Count - 1)
            {
                indexDirection++;
            }
            else
            {
                // reset back to the first direction
                indexDirection = 0;
            }
        }
        Console.WriteLine($"Answer for q1 is {q1}");


        // q2
        List<string> startingPoints = map.Keys.ToList().Where(x => x[2].Equals('A')).ToList();
        List<long> cycles = [];

        foreach (string start in startingPoints)
        {
            cycles.Add(FindFirstStop(start, directions, map));
        }

        // Find LCM
        long q2 = CalculateLCM(cycles[0], cycles[1]);
        for(int i = 2; i < cycles.Count; i++)
        {
            q2 = CalculateLCM(q2, cycles[i]);
        }

        Console.WriteLine($"Answer for q2 is {q2}");
    }

    public static long CalculateLCM(long firstNumber, long secondNumber)
    {
        long lcm = firstNumber * secondNumber / CalculateGCD(firstNumber, secondNumber);

        return lcm;
    }

    public static long CalculateGCD(long firstNumber, long secondNumber)
    {
        while (firstNumber != 0 && secondNumber != 0)
        {
            if (firstNumber > secondNumber)
            {
                firstNumber %= secondNumber;
            }
            else
            {
                secondNumber %= firstNumber;
            }
        }

        return firstNumber | secondNumber;
    }

    public static long FindFirstStop(string location, List<string> directions, Dictionary<string, Tuple<string, string>> map)
    {
        long steps = 0;
        int indexDirection = 0;
        while (!location[2].Equals('Z'))
        {
            Console.WriteLine($"Currently at {location}. Going {directions[indexDirection]}");

            location = directions[indexDirection] switch
            {
                "L" => map[location].Item1,
                "R" => map[location].Item2,
                _ => throw new ArgumentException($"Invalid direction {directions[indexDirection]}")
            };

            steps++;
            if (indexDirection < directions.Count - 1)
            {
                indexDirection++;
            }
            else
            {
                // reset back to the first direction
                indexDirection = 0;
            }
        }

        return steps;
    }

    // below functions did not scale

    public static List<string> UpdateLocations(List<string> input, Dictionary<string, Tuple<string, string>> map, List<string> directions, int indexDirection)
    {
        List<string> updatedLocations = [];
        foreach (string location in input)
        {
            string newLocation = directions[indexDirection] switch
            {
                "L" => map[location].Item1,
                "R" => map[location].Item2,
                _ => throw new ArgumentException($"Invalid direction {directions[indexDirection]}")
            };
            updatedLocations.Add(newLocation);
        }

        return updatedLocations;

    }

    public static bool CheckLocations(List<string> input)
    {
        bool allFinished = false;

        if (input.Where(x => x[2].Equals('Z')).ToList().Count == input.Count)
        {
            allFinished = true;
        }

        return allFinished;
    }
}