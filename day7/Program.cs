using System.Collections;
using System.Globalization;
using System.Security.Cryptography;

namespace day7;

class Program
{
    static void Main(string[] args)
    {
        StreamReader sr = new("input.txt");
        string? line;

        // Storing with cards as key and corresponding bid as value
        Dictionary<string, string> hands = [];
        while ((line = sr.ReadLine()) != null)
        {
            string[] splitted = line.Split(" ");
            hands[splitted[0]] = splitted[1];
        }

        // Finding strength of each hand
        // Tuple of rating (Item1),  hand (Item2) and bid (Item3)
        List<Tuple<int, string, int>> ratedHands = [];
        List<Tuple<int, string, int>> ratedWithJoker = [];

        foreach (KeyValuePair<string, string> hand in hands)
        {
            // Item1 of Tuple is which card and item2 is how many times you have it
            List<Tuple<char, int>> orderedCards = hand.Key.Select(m => m).ToList().GroupBy(m => m).Select(grp => new Tuple<char, int>(grp.Key, grp.Count())).ToList();

            // Rate the hand
            int rating = RateTheHand(orderedCards);
            int ratingWithJoker = RateTheHandWithJoker(orderedCards);
            ratedHands.Add(new Tuple<int, string, int>(rating, hand.Key, Convert.ToInt32(hand.Value)));
            ratedWithJoker.Add(new Tuple<int, string, int>(ratingWithJoker, hand.Key, Convert.ToInt32(hand.Value)));
        }

        // Sort all of the hands
        var orderedHands = ratedHands.GroupBy(m => m.Item1).ToList().OrderBy(grp => grp.Key).ToList();
        var orderedHandsWithJoker = ratedWithJoker.GroupBy(m => m.Item1).ToList().OrderBy(grp => grp.Key).ToList();

        // q1
        long q1 = CalculateQ1(orderedHands);
        Console.WriteLine($"Answer for q1 is {q1}.");

        // q2
        long q2 = 0;
        long handsEvaluated = 1;
        // replace face-values in item2 with number-values so that you can sort them
        for (int i = 0; i < orderedHandsWithJoker.Count; i++)
        {
            // Facevalue of your individual cards (Item1), numbervalue of your individual cards (Item2), bid (Item3)
            List<Tuple<string, string, int>> temp = [];
            List<Tuple<int, string, int>> cardsPerCategory = orderedHandsWithJoker[i].Select(m => m).ToList();
            for (int j = 0; j < cardsPerCategory.Count; j++)
            {
                temp.Add(new Tuple<string, string, int>(cardsPerCategory[j].Item2, CardValuesToNumberWithJoker(cardsPerCategory[j].Item2), cardsPerCategory[j].Item3));
            }

            // Order temp by Item2
            temp = temp.OrderBy(m => m.Item2).ToList();

            // Calculate answer per category
            foreach (Tuple<string, string, int> entry in temp)
            {
                q2 += (long)entry.Item3 * handsEvaluated;
                handsEvaluated++;
            }
        }
        Console.WriteLine($"Answer for q2 is {q2}.");
    }

    public static long CalculateQ1(List<IGrouping<int, Tuple<int, string, int>>> orderedHands)
    {
        long q1 = 0;
        long handsEvaluated = 1;
        // replace face-values in item2 with number-values so that you can sort them
        for (int i = 0; i < orderedHands.Count; i++)
        {
            // Facevalue of your individual cards (Item1), numbervalue of your individual cards (Item2), bid (Item3)
            List<Tuple<string, string, int>> temp = [];
            List<Tuple<int, string, int>> cardsPerCategory = orderedHands[i].Select(m => m).ToList();
            for (int j = 0; j < cardsPerCategory.Count; j++)
            {
                temp.Add(new Tuple<string, string, int>(cardsPerCategory[j].Item2, CardValuesToNumber(cardsPerCategory[j].Item2), cardsPerCategory[j].Item3));
            }

            // Order temp by Item2
            temp = temp.OrderBy(m => m.Item2).ToList();

            // Calculate answer per category
            foreach (Tuple<string, string, int> entry in temp)
            {
                q1 += (long)entry.Item3 * handsEvaluated;
                handsEvaluated++;
            }
        }

        return q1;
    }

    public static string CardValuesToNumber(string input)
    {
        string res = "";
        foreach (char val in input)
        {
            string valNumber = val switch
            {
                '2' => "01",
                '3' => "02",
                '4' => "03",
                '5' => "04",
                '6' => "05",
                '7' => "06",
                '8' => "07",
                '9' => "08",
                'T' => "09",
                'J' => "10",
                'Q' => "11",
                'K' => "12",
                'A' => "13",
                _ => throw new ArgumentException($"{val} is not a support card value")

            };

            res += valNumber;
        }
        return res;
    }
    public static string CardValuesToNumberWithJoker(string input)
    {
        string res = "";
        foreach (char val in input)
        {
            string valNumber = val switch
            {
                'J' => "01",
                '2' => "02",
                '3' => "03",
                '4' => "04",
                '5' => "05",
                '6' => "06",
                '7' => "07",
                '8' => "08",
                '9' => "09",
                'T' => "10",
                'Q' => "11",
                'K' => "12",
                'A' => "13",
                _ => throw new ArgumentException($"{val} is not a support card value")

            };

            res += valNumber;
        }
        return res;
    }


    public static int RateTheHand(List<Tuple<char, int>> orderedCards)
    {
        List<Tuple<char, int>> valuePairs = orderedCards.Where(m => m.Item2 > 1).ToList();

        if (valuePairs.Count != 0)
        {
            switch (valuePairs[0].Item2)
            {
                case 5:
                    return 6; // five of a kind
                case 4:
                    return 5; // four of a kind
                case 3:
                    if (valuePairs.Count == 2)
                    {
                        return 4; // Full house
                    }
                    else
                    {
                        return 3; // three of a kind
                    }
                case 2:
                    if (valuePairs.Count == 1)
                    {
                        return 1; // One pair
                    }
                    else if (valuePairs[1].Item2 == 3)
                    {
                        return 4; // Full house
                    }
                    else
                    {
                        return 2; // two pair
                    }
                default:
                    throw new ArgumentOutOfRangeException($"Too many cards on your hand? {valuePairs[0].Item2}");
            }
        }
        return 0; // High card
    }

    public static int RateTheHandWithJoker(List<Tuple<char, int>> orderedCards)
    {
        List<Tuple<char, int>> valuePairs = orderedCards.Where(m => m.Item2 > 1).OrderByDescending(m => m.Item2).ToList();

        // If double joker, remove it from valuePairs
        for (int i = 0; i < valuePairs.Count; i++)
        {
            if (valuePairs[i].Item1 == 'J')
            {
                valuePairs.RemoveAt(i);
            }
        }

        // Find number of jokers
        int numberOfJokers = 0;
        foreach (Tuple<char, int> entry in orderedCards)
        {
            if (entry.Item1 == 'J')
            {
                numberOfJokers = entry.Item2;
                if (valuePairs.Count != 0)
                {
                    // Add jokers to an existing value pair
                    Tuple<char, int> newPair = new Tuple<char, int>(valuePairs[0].Item1, valuePairs[0].Item2 + numberOfJokers);
                    valuePairs.RemoveAt(0);
                    valuePairs.Add(newPair);
                }
                else
                {
                    valuePairs.Add(new Tuple<char, int>(orderedCards[0].Item1, Math.Min(numberOfJokers + 1, 5)));
                }
            }
        }

        if (valuePairs.Count != 0)
        {
            switch (valuePairs[0].Item2)
            {
                case 5:
                    return 6; // five of a kind
                case 4:
                    return 5; // four of a kind
                case 3:
                    if (valuePairs.Count == 2)
                    {
                        return 4; // Full house
                    }
                    else
                    {
                        return 3; // three of a kind
                    }
                case 2:
                    if (valuePairs.Count == 1)
                    {
                        return 1; // One pair
                    }
                    else if (valuePairs[1].Item2 == 3)
                    {
                        return 4; // Full house
                    }
                    else
                    {
                        return 2; // two pair
                    }
                default:
                    throw new ArgumentOutOfRangeException($"Too many cards on your hand? {valuePairs[0].Item2}");
            }
        }
        return 0; // High card
    }


}
