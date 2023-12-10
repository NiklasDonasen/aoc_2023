using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;

namespace day10;

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

        // Find starting points
        Tuple<int, int> startingPoint = map.First(x => x.Value.Equals('S')).Key;

        // Starting options
        List<Tuple<int, int>> possibleStartingOptions = FindStartingOptions(startingPoint, map);
        Dictionary<Tuple<int, int>, int> stepsTaken = [];
        List<Tuple<int, int>> visited = [];

        // Do a Breadth first search (BFS) for each startinPoint
        foreach (Tuple<int, int> start in possibleStartingOptions)
        {
            // Breadth first search (BFS)
            visited = [];
            List<Tuple<int, int>> queue = [];

            // Mark the starting point as visited and enqueue it
            visited.Add(start);
            // Add location of 'S' so that you do not go there in the first round
            visited.Add(startingPoint);
            queue.Add(start);
            // You have already stepped one away from start
            int steps = 1;

            // Do the BFS-magic
            List<Tuple<int, int>> nextTiles = [];
            while (queue.Any())
            {
                // Dequeue from the start of the queue
                Tuple<int, int> location = queue.First();
                queue.RemoveAt(0);
                steps++;

                // Get possible adjacent tiles
                nextTiles = FindNextTiles(location, map);
                // If not visited before, enqueue them
                foreach (Tuple<int, int> tile in nextTiles)
                {
                    if (!visited.Contains(tile))
                    {
                        visited.Add(tile);
                        queue.Add(tile);
                    }
                }
            }
            if (nextTiles.Contains(startingPoint))
            {
                stepsTaken[start] = steps;
            }
        }

        int q1 = stepsTaken.Values.ToList().Max() / 2;
        Console.WriteLine($"Answer for q1 is {q1}");

        // q2
        int insideTheLoop = 0;

        // Set all tiles that are not a part of the loop with '.'
        foreach (KeyValuePair<Tuple<int, int>, char> entry in map)
        {
            if (!visited.Contains(entry.Key))
            {
                map[entry.Key] = '.';
            }
        }
        // Replace pipe with correct pipe - I hardcoded it
        map[startingPoint] = 'F';

        // Count number of times you cross a pipe, but start at the second row
        for (int row = 1; row < rowNumber; row++)
        {
            char currentPipe = 'X';
            bool inside = false;
            bool horizontal = false;
            for (int col = 0; col < colNumber; col++)
            {
                Tuple<int, int> toBeChecked = new Tuple<int, int>(row, col);
                char val = map[toBeChecked];

                if (!val.Equals('.'))
                {
                    // Crossing a pipe, but we have to check for compatibility to know if we are actually inside a loop
                    // J, L og |
                    // 7 og F
                    // to ganger samme kan lukke
                    if (currentPipe.Equals('X'))
                    {
                        // Going inside the loop
                        currentPipe = val;
                        inside = !inside;
                    }
                    else
                    {
                        switch (val)
                        {
                            case 'J':
                            case 'L':
                            case '|':
                                if ((currentPipe.Equals('J') || currentPipe.Equals('L') || currentPipe.Equals('|')) && !horizontal)
                                {
                                    currentPipe = val;
                                    inside = !inside;
                                    horizontal = false;
                                }
                                else if ((currentPipe.Equals('J') || currentPipe.Equals('L')) && horizontal)
                                {
                                    currentPipe = val;
                                    inside = !inside;
                                    horizontal = false;
                                }
                                break;
                            case 'F':
                            case '7':
                                if (currentPipe.Equals('F') || currentPipe.Equals('7'))
                                {
                                    currentPipe = val;
                                    inside = !inside;
                                    horizontal = false;
                                }
                                break;
                            case '-':
                                if (currentPipe.Equals('F') || currentPipe.Equals('7') || currentPipe.Equals('J') || currentPipe.Equals('L'))
                                {
                                    horizontal = true;
                                }
                                break;

                        }
                    }
                }
                else
                {
                    // You are either inside or outside the loop
                    if (inside)
                    {
                        // Don't take values at the edges
                        if (col < colNumber && row < rowNumber)
                        {
                            insideTheLoop++;
                        }
                    }
                }
            }

        }

        Console.WriteLine($"Answer to q2 is {insideTheLoop}");
        // 929 is not correct --> too high
        // 940 is too high

    }

    public static List<Tuple<int, int>> FindNextTiles(Tuple<int, int> location, Dictionary<Tuple<int, int>, char> map)
    {
        List<Tuple<int, int>> nextTiles = [];

        switch (map[location])
        {
            case '|':
                nextTiles.Add(new Tuple<int, int>(location.Item1 + 1, location.Item2));
                nextTiles.Add(new Tuple<int, int>(location.Item1 - 1, location.Item2));
                break;
            case '-':
                nextTiles.Add(new Tuple<int, int>(location.Item1, location.Item2 + 1));
                nextTiles.Add(new Tuple<int, int>(location.Item1, location.Item2 - 1));
                break;
            case 'L':
                nextTiles.Add(new Tuple<int, int>(location.Item1 - 1, location.Item2));
                nextTiles.Add(new Tuple<int, int>(location.Item1, location.Item2 + 1));
                break;
            case 'J':
                nextTiles.Add(new Tuple<int, int>(location.Item1, location.Item2 - 1));
                nextTiles.Add(new Tuple<int, int>(location.Item1 - 1, location.Item2));
                break;
            case '7':
                nextTiles.Add(new Tuple<int, int>(location.Item1, location.Item2 - 1));
                nextTiles.Add(new Tuple<int, int>(location.Item1 + 1, location.Item2));
                break;
            case 'F':
                nextTiles.Add(new Tuple<int, int>(location.Item1, location.Item2 + 1));
                nextTiles.Add(new Tuple<int, int>(location.Item1 + 1, location.Item2));
                break;
            case '.':
            case 'S':
                break;
        }

        for (int i = 0; i < nextTiles.Count; i++)
        {
            // Check if any tiles are out of bounce
            if (!map.ContainsKey(nextTiles[i]))
            {
                nextTiles.RemoveAt(i);
            }
        }

        return nextTiles;
    }

    public static List<Tuple<int, int>> FindStartingOptions(Tuple<int, int> location, Dictionary<Tuple<int, int>, char> map)
    {
        List<Tuple<int, int>> closeFields = [];
        char toBeEvaluated = 'X';

        bool top = map.TryGetValue(new Tuple<int, int>(location.Item1 - 1, location.Item2), out toBeEvaluated);
        if (toBeEvaluated.Equals('|') || toBeEvaluated.Equals('7') || toBeEvaluated.Equals('F'))
        {
            closeFields.Add(new Tuple<int, int>(location.Item1 - 1, location.Item2));
        }

        bool right = map.TryGetValue(new Tuple<int, int>(location.Item1, location.Item2 + 1), out toBeEvaluated);
        if (toBeEvaluated.Equals('-') || toBeEvaluated.Equals('J') || toBeEvaluated.Equals('7'))
        {
            closeFields.Add(new Tuple<int, int>(location.Item1, location.Item2 + 1));
        }

        bool bottom = map.TryGetValue(new Tuple<int, int>(location.Item1 + 1, location.Item2), out toBeEvaluated);
        if (toBeEvaluated.Equals('|') || toBeEvaluated.Equals('J') || toBeEvaluated.Equals('L'))
        {
            closeFields.Add(new Tuple<int, int>(location.Item1 + 1, location.Item2));
        }

        bool left = map.TryGetValue(new Tuple<int, int>(location.Item1, location.Item2 - 1), out toBeEvaluated);
        if (toBeEvaluated.Equals('-') || toBeEvaluated.Equals('L') || toBeEvaluated.Equals('F'))
        {
            closeFields.Add(new Tuple<int, int>(location.Item1, location.Item2 - 1));
        }

        // [
        //     // Add all close fields starting one above and going clockwise
        //     new Tuple<int, int>(location.Item1 - 1, location.Item2),
        //     // new Tuple<int, int>(location.Item1 - 1, location.Item2 + 1),
        //     new Tuple<int, int>(location.Item1, location.Item2 + 1),
        //     // new Tuple<int, int>(location.Item1 + 1, location.Item2 + 1),
        //     new Tuple<int, int>(location.Item1 + 1, location.Item2),
        //     // new Tuple<int, int>(location.Item1 + 1, location.Item2 - 1),
        //     new Tuple<int, int>(location.Item1, location.Item2 - 1),
        //     // new Tuple<int, int>(location.Item1 - 1, location.Item2 - 1),
        // ];

        // List<Tuple<int, int>> possibleFields = [];
        // for (int i = 0; i < closeFields.Count; i++)
        // {
        //     if (!map[closeFields[i]].Equals('.'))
        //     {
        //         possibleFields.Add(closeFields[i]);
        //     }
        // }

        return closeFields;
    }
}
