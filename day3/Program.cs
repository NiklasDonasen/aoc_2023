using System.Text.RegularExpressions;

namespace day3;

partial class Program
{
    static void Main(string[] args)
    {
        StreamReader sr = new("input.txt");

        List<int> q1 = [];
        int q2 = 0;
        Dictionary<List<Tuple<int, int>>, int> numbers = [];
        List<Tuple<int, int>> symbols = [];
        List<Tuple<int, int>> stars = [];


        string? line;
        int rowNumber = 0;
        while ((line = sr.ReadLine()) != null)
        {
            // Match all numbers
            MatchCollection matchedNumbers = Regex.Matches(line, @"\d+");
            for (int i = 0; i < matchedNumbers.Count; i++)
            {
                Tuple<int, List<Tuple<int, int>>> tempRes = StoreLocations(rowNumber, matchedNumbers[i]);
                numbers[tempRes.Item2] = tempRes.Item1;
            }
            // Match everything that is not a number or a dot
            MatchCollection matchedSymbols = Regex.Matches(line, @"[^0-9.]");
            for (int i = 0; i < matchedSymbols.Count; i++)
            {
                Tuple<int, List<Tuple<int, int>>> tempRes = StoreLocations(rowNumber, matchedSymbols[i]);
                symbols.AddRange(tempRes.Item2);
            }

            // Match everything that is only a star for q2
            MatchCollection matchedStars = Regex.Matches(line, @"[*]");
            for (int i = 0; i < matchedStars.Count; i++)
            {
                Tuple<int, List<Tuple<int, int>>> tempRes = StoreLocations(rowNumber, matchedStars[i]);
                stars.AddRange(tempRes.Item2);
            }

            // Increase row number ahead of line shift
            rowNumber++;
            Console.WriteLine("Done processing the file");
        }

        // q1
        foreach (var (locations, val) in numbers)
        {
            for (int i = 0; i < locations.Count(); i++)
            {
                if (CheckSymbolCloseness(locations[i], symbols))
                {
                    q1.Add(val);
                    break;
                }
            }
        }
        Console.WriteLine($"Answer to q1 is {q1.Sum()}");

        // q2
        for (int star = 0; star < stars.Count; star++)
        {
            List<int> adjacentNumbers = [];
            foreach (var (locations, val) in numbers)
            {
                if (CheckSymbolCloseness(stars[star], locations))
                {
                    adjacentNumbers.Add(val);
                }
            }
            // Check that it is only adjacent to two numbers
            if (adjacentNumbers.Count == 2)
            {
                q2 += adjacentNumbers[0] * adjacentNumbers[1];
            }
        }
        Console.WriteLine($"Answer to q2 is {q2}");
    }

    public static Tuple<int, List<Tuple<int, int>>> StoreLocations(int rowNumber, Match match)
    {
        List<Tuple<int, int>> tempList = [];

        int colStart = match.Index;
        int colStop = match.Index + match.Length;
        while (colStart < colStop)
        {
            tempList.Add(new Tuple<int, int>(rowNumber, colStart));
            colStart++;
        }
        // Return a tuple of the matching value as Item1 and the location as Item2
        int val = 0;
        bool matchIsNumber = Int32.TryParse(match.Value, out val);
        Tuple<int, List<Tuple<int, int>>> res = new(val, tempList);
        return res;
    }

    public static bool CheckSymbolCloseness(Tuple<int, int> location, List<Tuple<int, int>> symbols)
    {
        List<Tuple<int, int>> closeFields =
        [
            location,
            // Add all close fields starting one above and going clockwise
            new Tuple<int, int>(location.Item1 - 1, location.Item2),
            new Tuple<int, int>(location.Item1 - 1, location.Item2 + 1),
            new Tuple<int, int>(location.Item1, location.Item2 + 1),
            new Tuple<int, int>(location.Item1 + 1, location.Item2 + 1),
            new Tuple<int, int>(location.Item1 + 1, location.Item2),
            new Tuple<int, int>(location.Item1 + 1, location.Item2 - 1),
            new Tuple<int, int>(location.Item1, location.Item2 - 1),
            new Tuple<int, int>(location.Item1 - 1, location.Item2 - 1),
        ];

        if (closeFields.Intersect(symbols).Any())
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
