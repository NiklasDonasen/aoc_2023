using System.Runtime.InteropServices.Marshalling;
using System.Text.RegularExpressions;

namespace day13;

class Program
{
    static void Main(string[] args)
    {
        StreamReader sr = new("input.txt");
        string? line;

        // Store each pattern as a list of strings
        List<List<string>> patterns = [];
        List<string> temp = [];

        // q1
        int q1 = 0;

        while ((line = sr.ReadLine()) != null)
        {
            if (line != "")
            {
                // Replace for easier regex
                temp.Add(line.Replace(".", "A"));
            }
            else
            {
                patterns.Add(temp);
                temp = [];
            }

        }
        // Find possible splits
        List<int> indices = [];
        foreach (List<string> pattern in patterns)
        {
            // Try to find horizontal splits
            // Convert to cols
            List<string> newRows = [];
            for (int col = 0; col < pattern[0].Length; col++)
            {
                List<char> tempCol = [];
                for (int row = 0; row < pattern.Count; row++)
                {
                    tempCol.Add(pattern[row][col]);
                }
                newRows.Add(String.Join("", tempCol));
            }
            // Test for mirrors in first col
            indices = DeriveIndices(newRows);

            if (indices.Count == 1)
            {
                Console.WriteLine($"Found a horizontal split at {indices[0]}");
                q1 += indices[0] * 100;
            }
            else if (indices.Count > 1)
            {
                throw new ArgumentException("Too many possible splits");
            }
            else
            {
                // Try to find vertical splits
                indices = DeriveIndices(pattern);

                if (indices.Count == 1)
                {
                    Console.WriteLine($"Found a vertical split at {indices[0]}");
                    q1 += indices[0];
                }
                else if (indices.Count > 1)
                {
                    throw new ArgumentException("Too many possible splits");
                }
                else
                {
                    Console.WriteLine("Found no split. What???");
                }
            }

        }

        Console.WriteLine($"Answer for q1 is {q1}");
        // 12738 is too low
        // 29202 is too low
    }

    public static List<int> DeriveIndices(List<string> pattern)
    {
        // You only need to check the first row
        List<int> indices = FindMirrorIndices(pattern[0]).Distinct().ToList();

        // Test the split(s) from the first row on the rest
        for (int row = 1; row < pattern.Count; row++)
        {
            // Reverse order so that you can safely remove non-working indices
            for (int index = indices.Count - 1; index >= 0; index--)
            {
                // Test it
                string firstPart = pattern[row].Substring(0, indices[index]);
                string secondPart = pattern[row].Substring(indices[index]);

                bool indexWorks = CheckForMirror(firstPart, secondPart);
                if (!indexWorks)
                {
                    indices.RemoveAt(index);
                }
            }
        }

        return indices;
    }

    public static List<int> FindMirrorIndices(string row)
    {
        List<int> indices = [];

        // Start at the first possible mirror-line
        for (int i = 1; i < row.Length; i++)
        {
            string firstPart = row.Substring(0, i);
            string secondPart = row.Substring(i);

            if (CheckForMirror(firstPart, secondPart))
            {
                indices.Add(i);
            }

        }
        return indices;
    }

    public static bool CheckForMirror(string firstPart, string secondPart)
    {
        // Either drop characters in the beginning or in the end of the row
        if (firstPart.Length > secondPart.Length)
        {
            firstPart = firstPart.Substring(0 + firstPart.Length - secondPart.Length);
        }
        else if (firstPart.Length < secondPart.Length)
        {
            secondPart = secondPart.Substring(0, firstPart.Length);
        }
        if (firstPart == "" && secondPart == "")
        {
            return false;
        }
        secondPart = string.Join("", secondPart.ToCharArray().Reverse());
        if (firstPart == secondPart)
        {
            return true;
        }
        return false;
    }

}

