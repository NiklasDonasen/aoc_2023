using System.ComponentModel;

namespace day11;

class Program
{
    static void Main(string[] args)
    {
        StreamReader sr = new("input.txt");
        string? line;
        int rowNumber = 0;
        int colNumber = 0;

        // (row, col) as key
        Dictionary<Tuple<int, int>, char> map = [];

        while ((line = sr.ReadLine()) != null)
        {
            List<char> chars = line.Select(m => m).ToList();
            for (int i = 0; i < chars.Count; i++)
            {
                map[new Tuple<int, int>(rowNumber, i)] = chars[i];
            }

            // Expand empty rows
            if (!line.Contains("#"))
            {
                rowNumber++;
                for (int i = 0; i < chars.Count; i++)
                {
                    map[new Tuple<int, int>(rowNumber, i)] = '.';
                }
            }

            rowNumber++;
            colNumber = chars.Count;
        }

        // Expand empty columns without overwriting existing values
        Dictionary<Tuple<int, int>, char> newMap = [];
        int extraCols = 0;
        for (int col = 0; col < colNumber; col++)
        {
            var tempCol = map.Where(m => m.Key.Item2 == col - extraCols && m.Value.Equals('.')).ToList();
            if (!(tempCol.Count == rowNumber))
            {
                // We have at least one galaxy in this row and you can simply copy over the entire column
                for (int i = 0; i < rowNumber; i++)
                {
                    newMap[new Tuple<int, int>(i, col)] = map[new Tuple<int, int>(i, col - extraCols)];
                }
            }
            else
            {
                // Copy it in once
                for (int i = 0; i < rowNumber; i++)
                {
                    newMap[new Tuple<int, int>(i, col)] = '.';
                }
                colNumber++;
                col++;
                extraCols++;
                // Copy it in a second time, but with one higher col-value
                for (int i = 0; i < rowNumber; i++)
                {
                    newMap[new Tuple<int, int>(i, col)] = '.';
                }
            }
        }

        // Find position of galaxies
        List<Tuple<int, int>> galaxies = newMap.Where(m => m.Value.Equals('#')).Select(m => new Tuple<int, int>(m.Key.Item1, m.Key.Item2)).ToList();

        // Find all galaxy-pairs
        List<List<Tuple<int, int>>> galaxyPairs = [];
        while (galaxies.Count > 0)
        {
            for (int i = 0; i < galaxies.Count - 1; i++)
            {
                galaxyPairs.Add([galaxies[i], galaxies[i + 1]]);
            }
            galaxies.RemoveAt(0);
        }

        // Find the shortest path between each pair
        int q1 = 0;
        foreach (List<Tuple<int, int>> tuplePair in galaxyPairs)
        {
            q1 += shortestPath()
        }


        Console.WriteLine($"Answer for q1 is {q1}");



    }

    public static int shortestPath(Tuple<int, int> firstPair, Tuple<int, int> secondPair)
    {
        int horizontalSteps = Math.Abs(firstPair.Item2 - secondPair.Item2);
        int verticalSteps = Math.Abs(firstPair.Item1 - secondPair.Item2);

        return horizontalSteps + verticalSteps;
    }
}
