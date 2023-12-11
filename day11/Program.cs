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

            rowNumber++;
            colNumber = chars.Count;
        }

        // Find empty rows
        List<int> emptyRows = [];
        for (int row = 0; row < rowNumber; row++)
        {
            var tempRow = map.Where(m => m.Key.Item1 == row && m.Value.Equals('.')).ToList();
            if (tempRow.Count == colNumber)
            {
                emptyRows.Add(row);
            }
        }
        // Find empty columns
        List<int> emptyCols = [];
        for (int col = 0; col < colNumber; col++)
        {
            var tempCol = map.Where(m => m.Key.Item2 == col && m.Value.Equals('.')).ToList();
            if (tempCol.Count == rowNumber)
            {
                emptyCols.Add(col);
            }
        }

        // Find position of galaxies
        List<Tuple<int, int>> galaxies = map.Where(m => m.Value.Equals('#')).Select(m => new Tuple<int, int>(m.Key.Item1, m.Key.Item2)).ToList();

        // Find all galaxy-pairs
        var galaxyPairs = galaxies.SelectMany((first, i) => galaxies.Skip(i + 1).Select(second => (first, second))).ToList();

        // // Find the shortest path between each pair
        long q = 0;
        int extraPadding = 999999;
        foreach (var tuplePair in galaxyPairs)
        {
            q += PathPlusPadding(tuplePair.first, tuplePair.second, extraPadding, emptyRows, emptyCols);
        }

        Console.WriteLine($"Answer with {extraPadding}x extra padding is {q}");

    }

    public static long PathPlusPadding(Tuple<int, int> firstPair, Tuple<int, int> secondPair, long extraPadding, List<int> emptyRows, List<int> emptyCols)
    {
        // Get the vertical positions you are going through
        List<int> verticalSteps = [];
        if (firstPair.Item1 > secondPair.Item1)
        {
            verticalSteps = Enumerable.Range(secondPair.Item1, firstPair.Item1 - secondPair.Item1).ToList();
        }
        else
        {
            verticalSteps = Enumerable.Range(firstPair.Item1, secondPair.Item1 - firstPair.Item1).ToList();
        }

        // Get the horizontal positions you are going through
        List<int> horizontalSteps = [];
        if (firstPair.Item2 > secondPair.Item2)
        {
            horizontalSteps = Enumerable.Range(secondPair.Item2, firstPair.Item2 - secondPair.Item2).ToList();
        }
        else
        {
            horizontalSteps = Enumerable.Range(firstPair.Item2, secondPair.Item2 - firstPair.Item2).ToList();
        }

        int ordinarySteps = horizontalSteps.Count + verticalSteps.Count;

        // Keep track of empty rows or columns that you pass through
        int counter = 0;

        foreach (int number in horizontalSteps)
        {
            if (emptyCols.Contains(number))
            {
                counter++;
            }
        }

        foreach (int number in verticalSteps)
        {
            if (emptyRows.Contains(number))
            {
                counter++;
            }
        }

        return (long)ordinarySteps + (long)counter * extraPadding;

    }

    public static int ShortestPath(Tuple<int, int> firstPair, Tuple<int, int> secondPair)
    {
        int horizontalSteps = Math.Abs(firstPair.Item2 - secondPair.Item2);
        int verticalSteps = Math.Abs(firstPair.Item1 - secondPair.Item1);

        return horizontalSteps + verticalSteps;
    }
}
