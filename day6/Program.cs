using System.Text.RegularExpressions;
namespace day6;

class Program
{
    static void Main(string[] args)
    {
        StreamReader sr = new("input.txt");
        string? line;

        // q1
        List<long> numberOfFasterRecords = [];

        // q2
        string timeQ2 = "";
        string distanceQ2 = "";

        // Store games as time, distance pairs
        List<Tuple<long, long>> trackRecord = [];

        List<long> timesList = [];
        List<long> distanceList = [];

        while ((line = sr.ReadLine()) != null)
        {
            string[] splitted = line.Split(":");
            if (splitted[0] == "Time")
            {
                MatchCollection times = Regex.Matches(splitted[1], @"\d+");
                timesList = times.Select(m => (long)Convert.ToDouble(m.Value)).ToList();

                // q2
                timeQ2 = splitted[1].Replace(" ", String.Empty);

            }
            else if (splitted[0] == "Distance")
            {
                MatchCollection distances = Regex.Matches(splitted[1], @"\d+");
                distanceList = distances.Select(m => (long)Convert.ToInt32(m.Value)).ToList();

                // q2
                distanceQ2 = splitted[1].Replace(" ", String.Empty);
            }
        }

        // Track record
        for (int i = 0; i < timesList.Count; i++)
        {
            trackRecord.Add(new Tuple<long, long>(timesList[i], distanceList[i]));
        }

        foreach (Tuple<long, long> record in trackRecord)
        {
            long fasterRecords = FindPossibleRecords(record);
            if (fasterRecords > 0)
            {
                numberOfFasterRecords.Add(fasterRecords);
            }
        }

        long q1 = numberOfFasterRecords.Aggregate((a, x) => a * x);
        Console.WriteLine($"Answer to q1 is {q1}");

        // q2
        Tuple<long, long> q2Record = new Tuple<long, long>((long)Convert.ToDouble(timeQ2), (long)Convert.ToDouble(distanceQ2));
        double q2 = FindPossibleRecords(q2Record);
        Console.WriteLine($"Answer to q2 is {q2}");

    }
    public static long FindPossibleRecords(Tuple<long, long> curRec)
    {
        long numberOfWaysToBeatRecord = 0;
        long maxTime = curRec.Item1;
        long distanceToBeat = curRec.Item2;

        long boatSpeed = 0;
        for (long holdButton = 0; holdButton < maxTime; holdButton++)
        {
            boatSpeed = holdButton;
            long remainingTime = maxTime - holdButton;

            long distanceReached = boatSpeed * remainingTime;

            if (distanceReached > distanceToBeat)
            {
                numberOfWaysToBeatRecord++;
            }
        }

        return numberOfWaysToBeatRecord;
    }
}
