using System.Globalization;
using System.Text.RegularExpressions;
namespace day4;

class Program
{
    static void Main(string[] args)
    {
        StreamReader sr = new("input.txt");
        string? line;

        int q1 = 0;
        int q2 = 0;
        // Card number as key, and "how many times you have the card" as val
        Dictionary<int, int> winningsPerCard = [];
        // Instantiate the dictionary for q2 with 1 instance per card
        for (int i = 1; i <= 204; i++)
        {
            winningsPerCard[i] = 1;
        }

        while ((line = sr.ReadLine()) != null)
        {
            string[] cards = line.Split(":");
            int cardNumber = Convert.ToInt32(Regex.Match(cards[0], @"\d+").Value);
            Console.WriteLine($"Working with card {cardNumber}");

            string[] games = cards[1].Split("|");
            MatchCollection winningMatches = Regex.Matches(games[0], @"\d+");
            List<int> winningNumbers = (List<int>)winningMatches.Select(m => Convert.ToInt32(m.Value)).ToList();

            MatchCollection yourMatches = Regex.Matches(games[1], @"\d+");
            List<int> yourNumbers = (List<int>)yourMatches.Select(m => Convert.ToInt32(m.Value)).ToList();

            // Check overlap
            List<int> commonNumbers = (List<int>)winningNumbers.Intersect(yourNumbers).Select(m => m).ToList();

            // // q1
            int lineAnswer = 0;
            if (commonNumbers.Count() > 0)
            {
                lineAnswer = 1;
                commonNumbers.RemoveAt(0);
            }
            foreach (int val in commonNumbers)
            {
                lineAnswer *= 2;
            }

            // Add to answer
            q1 += lineAnswer;

            // q2
            // Loop over original and all copies of the card
            for (int j = 0; j < winningsPerCard[cardNumber]; j++)
            {
                // Loop over the winnings of an individual card
                for (int i = 1; i <= commonNumbers.Count; i++)
                {
                    winningsPerCard[cardNumber + i] = winningsPerCard[cardNumber + i] + 1;
                }
            }
        }

        Console.WriteLine($"Answer for q1 is {q1}");

        q2 = winningsPerCard.Values.Sum();
        Console.WriteLine($"Answer for q2 is {q2}");
    }
}
