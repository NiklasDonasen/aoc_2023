using System.Text.RegularExpressions;

namespace day2;

class Program
{
    static void Main()
    {
        StreamReader sr = new("input.txt");

        int q1 = 0;
        int q2 = 0;

        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            string[] groups = line.Split(":");
            Match match = Regex.Match(groups[0], @"\d+");
            int gameNumber = Convert.ToInt32(match.Value);
            string[] games = groups[1].Split(";");

            // q1
            bool withinLimits = true;
            for (int i = 0; i < games.Length; i++)
            {
                string[] colours = games[i].Split(",");
                for (int j = 0; j < colours.Length; j++)
                {
                    withinLimits = CheckLimits(colours[j]);
                    if (withinLimits == false)
                    {
                        Console.WriteLine($"Game {colours[j]} is not valid");
                        break;
                    }
                }
                if (withinLimits == false)
                {
                    Console.WriteLine($"Game {gameNumber} is not valid");
                    break;
                }
            }
            if (withinLimits == true)
            {
                q1 += gameNumber;
            }

            // q2
            Dictionary<string, int> res = new()
            {
                { "red", 0 },
                { "green", 0 },
                { "blue", 0 }
            };
            for (int i = 0; i < games.Length; i++)
            {
                string[] colours = games[i].Split(",");
                for (int j = 0; j < colours.Length; j++)
                {
                    res = CheckNumber(colours[j], res);
                }
            }

            int temp = CalculateResult(res);

            Console.WriteLine($"Answer for game {gameNumber} is {temp}");
            q2 += temp;

        }
        Console.WriteLine($"Answer for q1 is {q1}.");
        Console.WriteLine($"Answer for q2 is {q2}.");

    }

    public static Dictionary<string, int> CheckNumber(string input, Dictionary<string, int> res)
    {
        string[] splitted = input.Split(" ");
        int newVal = Convert.ToInt32(splitted[1]);
        if (res[splitted[2]] < newVal)
        {
            res[splitted[2]] = newVal;
        }
        return res;
    }

    public static int CalculateResult(Dictionary<string, int> res)
    {
        int temp = 1;
        foreach (string key in res.Keys)
        {
            if (res[key] == 0)
            {
                Console.WriteLine("Uuiuiui. Zero cubes??");
            }
            temp *= res[key];
        }
        return temp;
    }

    public static bool CheckLimits(string input)
    {
        bool withinLimits = true;
        string[] splitted = input.Split(" ");
        string colour = splitted[2];
        switch (colour)
        {
            case "red":
                if (Convert.ToInt32(splitted[1]) > 12)
                    withinLimits = false;
                break;
            case "green":
                if (Convert.ToInt32(splitted[1]) > 13)
                    withinLimits = false;
                break;
            case "blue":
                if (Convert.ToInt32(splitted[1]) > 14)
                    withinLimits = false;
                break;
            default:
                throw new ArgumentException($"Invalid colour {colour}");
        }

        return withinLimits;
    }
}
