using System.Text;
namespace day15;

class Program
{
    static void Main(string[] args)
    {
        StreamReader sr = new("input.txt");
        string? line;
        List<string> input = [];
        double q1 = 0;

        while ((line = sr.ReadLine()) != null)
        {
            input = line.Split(",").Select(m => m).ToList();
        }

        foreach (string step in input)
        {
            q1 += CalculateHashAlgorithm(step);
        }

        Console.WriteLine($"Answer to q1 is {q1}");
    }

    public static double CalculateHashAlgorithm(string input)
    {
        double stepValue = 0;

        // split the input and ignore newline characters
        char[] elements = input.Replace("\\n", "").Select(m => m).ToArray();

        // Convert to ASCII
        List<double> asciiValues = Encoding.Default.GetBytes(elements).Select(m => Convert.ToDouble(m)).ToList();

        foreach (double element in asciiValues)
        {
            // Increase the current value by the ASCII code you just determined
            stepValue += element;

            // Set the current value to itself multiplied by 17.
            stepValue *= 17;

            // Set the current value to the remainder of dividing itself by 256.
            stepValue = stepValue % 256;
        }

        return stepValue;
    }
}
