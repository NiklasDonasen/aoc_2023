using System.Text.RegularExpressions;
namespace day1;

class Program
{
    static void Main(string[] args)
    {

        StreamReader sr = new StreamReader("input.txt");

        List<int> q1 = new List<int>();
        List<int> q2 = new List<int>();

        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            Console.WriteLine($"Working with {line}");
            List<string> temp = new List<string>();
            for (int i = 0; i < line.Length; i++)
            {
                if (Char.IsDigit(line[i]))
                {
                    temp.Add(Convert.ToString(Char.GetNumericValue(line[i])));
                }
            }
            string lineAnswer1 = temp[0] + temp.Last();
            Console.WriteLine($"Adding {lineAnswer1}");
            q1.Add(Convert.ToInt32(lineAnswer1));

            // q2
            string numbersAndWords = @"(one|1|two|2|three|3|four|4|five|5|six|6|seven|7|eight|8|nine|9)";

            MatchCollection matcher = Regex.Matches(line, numbersAndWords);
            MatchCollection matcherRightLeft = Regex.Matches(line, numbersAndWords, RegexOptions.RightToLeft);

            string lineAnswer = "";
            Console.WriteLine($"First match is {matcher[0]}");
            string start = matcher[0].Value;
            lineAnswer = UnderstandWordsAndNumbers(lineAnswer, start);

            string? end = "";
            if (matcher.Last() != matcherRightLeft[0])
            {
                end = matcherRightLeft[0].Value;
            }
            else
            {
                end = matcher.Last().Value;
            }

            Console.WriteLine($"Last match is {end}");
            lineAnswer = UnderstandWordsAndNumbers(lineAnswer, end);
            q2.Add(Convert.ToInt32(lineAnswer));
        }

        int res_q1 = q1.Sum();
        Console.WriteLine($"Answer to q1 is {res_q1}");

        int res_q2 = q2.Sum();
        Console.WriteLine($"Answer to q2 is {res_q2}");

    }

    public static string UnderstandWordsAndNumbers(string lineAnswer, string match)
    {
        switch (match)
        {
            case "1":
            case "one":
                lineAnswer += "1";
                break;
            case "2":
            case "two":
                lineAnswer += "2";
                break;
            case "3":
            case "three":
                lineAnswer += "3";
                break;
            case "4":
            case "four":
                lineAnswer += "4";
                break;
            case "5":
            case "five":
                lineAnswer += "5";
                break;
            case "6":
            case "six":
                lineAnswer += "6";
                break;
            case "7":
            case "seven":
                lineAnswer += "7";
                break;
            case "8":
            case "eight":
                lineAnswer += "8";
                break;
            case "9":
            case "nine":
                lineAnswer += "9";
                break;
        }

        return lineAnswer;
    }
}
