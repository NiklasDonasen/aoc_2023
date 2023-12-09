using System.Text.RegularExpressions;
namespace day9;

class Program
{
    static void Main(string[] args)
    {
        StreamReader sr = new("input.txt");
        long q1 = 0;
        long q2 = 0;
        string? line;
        int lineNumber = 0;
        Dictionary<int, List<long>> sequences = [];

        while ((line = sr.ReadLine()) != null)
        {
            List<long> numbers = Regex.Matches(line, @"-?\d+").Select(x => (long)Convert.ToDouble(x.Value)).ToList();
            sequences[lineNumber] = numbers;
            lineNumber++;
        }

        // q1
        foreach (KeyValuePair<int, List<long>> sequence in sequences)
        {
            // Get all nested levels
            Dictionary<int, List<long>> tempDict = GetToZero(sequence.Value);

            // Now you can work your way up the nested levels
            long toBeAdded = 0;
            for (int i = tempDict.Keys.Count - 1; i >= 0; i--)
            {
                long curLastValue = tempDict[i].Last();
                long newLastValue = curLastValue + toBeAdded;
                toBeAdded = newLastValue;
            }

            q1 += toBeAdded;
        }
        Console.WriteLine($"Answer for q1 is {q1}.");

        // q2
        foreach (KeyValuePair<int, List<long>> sequence in sequences)
        {
            // Get all nested levels
            Dictionary<int, List<long>> tempDict = GetToZero(sequence.Value);

            // Now you can work your way up the nested levels
            long toBeAdded = 0;
            for (int i = tempDict.Keys.Count - 1; i >= 0; i--)
            {
                long curFirstValue = tempDict[i][0];
                long newFirstValue = curFirstValue - toBeAdded;
                toBeAdded = newFirstValue;
            }

            q2 += toBeAdded;
        }
        Console.WriteLine($"Answer for q2 is {q2}.");

    }

    public static Dictionary<int, List<long>> GetToZero(List<long> sequence)
    {
        Dictionary<int, List<long>> tempDict = new(){
            {0, sequence}
        };

        List<long> differences = CalculateDifference(sequence);
        int nestedLevels = 1;
        tempDict[nestedLevels] = differences;

        if (differences.Sum() != 0)
        {
            bool reachedZero = false;
            while (!reachedZero)
            {
                differences = CalculateDifference(tempDict[nestedLevels]);

                nestedLevels++;
                tempDict[nestedLevels] = differences;

                long sumOfDifferences = 0;
                foreach (long val in differences)
                {
                    sumOfDifferences += Math.Abs(val);
                }
                if (sumOfDifferences == 0)
                {
                    reachedZero = true;
                }
            }
        }

        return tempDict;
    }

    public static List<long> CalculateDifference(List<long> sequence)
    {
        List<long> differences = [];
        for (int i = 1; i < sequence.Count; i++)
        {
            differences.Add(sequence[i] - sequence[i - 1]);
        }
        return differences;
    }
}
