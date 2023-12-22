using System.Text.RegularExpressions;

namespace day5;

class Program
{
    static void Main()
    {

        StreamReader sr = new("input.txt");
        string? line;

        // seed number as key, and location as value
        Dictionary<double, double> seedLocation = [];

        // temp dict
        Dictionary<double, double> tempMap = [];

        // all seeds
        List<double> seedNumbers = [];

        double q1and2 = 0;
        // double q2 = 0;
        int lineNumber = 0;

        while ((line = sr.ReadLine()) != null)
        {
            lineNumber++;
            Console.WriteLine($"Working on line {lineNumber}");
            if (line.Contains("seeds: "))
            {
                // first line
                string[] splitted = line.Split(":");
                MatchCollection matcher = Regex.Matches(splitted[1], @"\d+");

                for (int i = 0; i < matcher.Count; i++)
                {
                    // Get all seeds
                    double seed = Convert.ToDouble(matcher[i].Value);
                    seedNumbers.Add(seed);

                    // q1
                    // seedLocation[seed] = seed;

                }

                // q2
                List<double> rangeSeeds = [];
                for (int j = 0; j < seedNumbers.Count - 1; j++)
                {
                    double start = seedNumbers[j];
                    double range = seedNumbers[j + 1];
                    for (double step = 0; step < range; step++)
                    {
                        rangeSeeds.Add(start + step);
                    }
                    j++;
                }
                foreach (double seed in rangeSeeds)
                {
                    seedLocation[seed] = seed;
                }

                continue;
            }
            if (line.Contains("map"))
            {
                continue;
            }
            // if numbers, populate the temporary dict
            if (line != "")
            {
                tempMap = parseNumbers(line, tempMap, seedLocation.Values.Select(m => m).ToList());
            }
            // if empty line, then update the real dict
            else
            {
                if (tempMap.Keys.Count != 0)
                {
                    seedLocation = updateValues(seedLocation, tempMap);
                    tempMap = [];
                }
            }
        }

        q1and2 = seedLocation.Values.Min();
        Console.WriteLine($"Answer is {q1and2}");
    }

    public static Dictionary<double, double> updateValues(Dictionary<double, double> curDict, Dictionary<double, double> tempDict)
    {
        // Now we can loop over all key-value pairs in tempDict and update curDict
        foreach (KeyValuePair<double, double> entry in curDict)
        {
            double tempKey = entry.Value;
            try
            {
                // See if you find a corresponding key in tempDict
                curDict[entry.Key] = tempDict[tempKey];
            }
            catch (KeyNotFoundException)
            {
                curDict[entry.Key] = tempKey;
            }
        }
        return curDict;
    }

    public static Dictionary<double, double> parseNumbers(string input, Dictionary<double, double> valueMap, List<double> seedLocations)
    {
        string[] splitted = input.Split(" ");
        double destStart = Convert.ToDouble(splitted[0]);
        double srcStart = Convert.ToDouble(splitted[1]);
        double rangeLength = Convert.ToDouble(splitted[2]);

        // TODO: fertilizer to water is not working
        foreach (double seed in seedLocations)
        {
            if (seed >= srcStart && seed < srcStart + rangeLength)
            {
                valueMap[seed] = destStart + (seed - srcStart);
            }
        }
        return valueMap;
    }
}
