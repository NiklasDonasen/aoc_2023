using System.Text.RegularExpressions;

namespace day5;

class Program
{
    static void Main()
    {

        StreamReader sr = new("input.txt");
        string? line;

        // seed number as key, and location as value
        Dictionary<long, long> seedLocation = [];

        // temp dict
        Dictionary<long, long> tempMap = [];

        // all seeds
        List<long> seedNumbers = [];

        // // You have to populate dictionary with all values
        // long biggestSeed = 0;

        long q1 = 0;
        // long q2 = 0;
        string currentMap = "";
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
                    seedNumbers.Add((long)Convert.ToDouble(matcher[i].Value));
                    // // Find the biggest seed
                    // biggestSeed = seedNumber > biggestSeed ? seedNumber : biggestSeed;
                }
            }
            else if (line == "")
            {
                if (currentMap != "" && currentMap != "seed-to-soil")
                {
                    // Update Values based on new tempMap
                    seedLocation = updateValues(seedLocation, tempMap);
                }
                // empty line
                // Reset the tempMap
                tempMap = [];
                continue;
            }
            else if (line.Contains(" map:"))
            {
                string[] splitted = line.Split(" ");
                currentMap = splitted[0];
            }
            else if (lineNumber == 33)
            {
                seedLocation = updateValues(seedLocation, tempMap, seedNumbers);
            }
            else
            {
                // Parse the numbers
                tempMap = parseNumbers(line, tempMap);
                // biggestSeed = tempMap.Keys.Max() > biggestSeed ? tempMap.Keys.Max() : biggestSeed;

                if (currentMap == "seed-to-soil")
                {
                    foreach (KeyValuePair<long, long> entry in seedLocation)
                    {
                        if (tempMap.TryGetValue(entry.Key, out long value))
                        {
                            seedLocation[entry.Key] = value;
                        }
                    }
                }
            }
        }

        q1 = seedLocation.Values.Min();
        Console.WriteLine($"Answer to q1 is {q1}");
    }

    public static Dictionary<long, long> updateValues(Dictionary<long, long> curDict, Dictionary<long, long> tempDict, List<long> seedNumbers)
    {
        // curDict maps seed to soil
        // tempDict maps soil to fertilizer

        // Loop over all seeds
        foreach (long seed in seedNumbers)
        {
            // seed is now the key in curDict
        }

        // Now we can loop over all key-value pairs in tempDict and update curDict
        foreach (KeyValuePair<long, long> entry in curDict)
        {
            long tempKey = entry.Value;
            try
            {
                curDict[entry.Key] = tempDict[tempKey];
            }
            catch (KeyNotFoundException)
            {
                curDict[entry.Key] = tempKey;
            }
            // // Find curKey based on tempKey
            // int curKey = curDict.FirstOrDefault(x => x.Value == tempEntry.Key).Key;
            // curDict[curKey] = tempEntry.Value;
        }
        return curDict;
    }

    public static Dictionary<long, long> parseNumbers(string input, Dictionary<long, long> valueMap)
    {
        string[] splitted = input.Split(" ");
        long destStart = (long)Convert.ToDouble(splitted[0]);
        long srcStart = (long)Convert.ToDouble(splitted[1]);
        long rangeLength = (long)Convert.ToDouble(splitted[2]);

        while (rangeLength > 0)
        {
            valueMap[srcStart] = destStart;
            srcStart++;
            destStart++;
            rangeLength--;
        }

        // // Populate all other keys between 0 and biggestSeed
        // for (long i = 0; i <= biggestSeed; i++)
        // {
        //     if (!valueMap.ContainsKey(i))
        //     {
        //         valueMap[i] = i;
        //     }
        // }

        return valueMap;
    }
}
