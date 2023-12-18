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
        // You only need to check the first row and then you need to test the split(s) you found on the rest

        // Find possible splits based on first row
        foreach (List<string> pattern in patterns)
        {
            foreach (string row in pattern)
            {
                // TODO: you may get several splits per row. Take the one from the first row? or take all from each row and see which fits all rows?
                int index = FindMirrorIndex(row);
            }
        }
    }

    public static int FindMirrorIndex(string row)
    {
        int index = -10;
        bool mirror = false;
        // One less than length since you are looking current + next character
        for (int i = 0; i < row.Length - 1; i++)
        {
            // Reverse since we need a mirror image
            string toBeMatched = row.Substring(i, 2);
            string matchReversed = string.Join("", toBeMatched.ToCharArray().Reverse());
            MatchCollection matches = Regex.Matches(row, matchReversed);

            if (matches.Count == 1 && matches[0].Index == i)
            {
                // no matching sequence
                continue;
            }

            for (int match = matches.Count - 1; match >= 0; match--)
            {
                // Start is always the first match
                int start = i + 2;
                int stop = matches[match].Index;
                if (stop - start < 0)
                {
                    // two matches share some of the same characters
                    break;
                }
                string toBeEvaluated = row.Substring(start, stop - start);

                // Check feasibility
                if (toBeEvaluated.Length % 2 != 0)
                {
                    // String cannot be equally mirroed
                    continue;
                }
                else
                {
                    mirror = CheckForMirror(toBeEvaluated);
                    if (mirror)
                    {
                        index = start + (stop - start) / 2;
                    }
                    break;
                }
            }
            if (mirror)
            {
                // Found a mirror in this row. Onto the next row
                break;
            }
        }

        return index;
    }

    public static bool CheckForMirror(string input)
    {
        string firstPart = input.Substring(0, input.Length / 2);
        string secondPart = input.Substring(input.Length / 2, input.Length - input.Length / 2);
        secondPart = string.Join("", secondPart.ToCharArray().Reverse());
        if (firstPart == secondPart)
        {
            return true;
        }
        return false;
    }
}
