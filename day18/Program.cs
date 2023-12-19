using System.Dynamic;
using System.Text.RegularExpressions;

namespace day18;

class Program
{
    static void Main(string[] args)
    {
        StreamReader sr = new("input.txt");
        string? line;

        List<Tuple<double, double>> holes = [];
        List<Tuple<double, double>> holesQ2 = [];

        double row = 0;
        double col = 0;
        holes.Add(new Tuple<double, double>(row, col));
        holesQ2.Add(new Tuple<double, double>(row, col));
        double borderLength = 0;
        double borderLengthQ2 = 0;


        while ((line = sr.ReadLine()) != null)
        {
            string[] splitted = line.Split(" ");

            // q1
            holes = ParseInput(splitted[0], Convert.ToInt32(splitted[1]), holes);
            borderLength += Convert.ToInt32(splitted[1]);

            // q2
            // Parsing slightly different input
            string code = splitted[2].Replace("(", "").Replace(")", "").Replace("#", "");
            string direction = code.Last().ToString();
            double steps = Convert.ToDouble(int.Parse(code.Substring(0, code.Length - 1), System.Globalization.NumberStyles.HexNumber));
            borderLengthQ2 += steps;
            holesQ2 = ParseInput(direction, steps, holesQ2);
        }

        // q1
        double areaQ1 = CalculateArea(holes);
        double q1 = areaQ1 + borderLength / 2 + 1;
        Console.WriteLine($"Answer for q1 is {q1}");

        // q2
        double areaQ2 = CalculateArea(holesQ2);
        double q2 = areaQ2 + borderLengthQ2 / 2 + 1;
        Console.WriteLine($"Answer for q2 is {q2}");

    }

    public static double CalculateArea(List<Tuple<double, double>> holes)
    {
        int n = holes.Count;
        double a = 0.0;

        for (int i = 0; i < n - 1; i++)
        {
            a += holes[i].Item1 * holes[i + 1].Item2 - holes[i + 1].Item1 * holes[i].Item2;
        }

        double area = Math.Abs(a + holes[n - 1].Item1 * holes[0].Item2 - holes[0].Item1 * holes[n - 1].Item2) / 2.0;

        return area;
    }

    public static List<Tuple<double, double>> ParseInput(string direction, double steps, List<Tuple<double, double>> holes)
    {

        Tuple<double, double> curPos = holes.Last();
        switch (direction)
        {
            case "0":
            case "R":
                holes.Add(new Tuple<double, double>(curPos.Item1, curPos.Item2 + steps));
                return holes;
            case "1":
            case "D":
                holes.Add(new Tuple<double, double>(curPos.Item1 + steps, curPos.Item2));
                return holes;
            case "2":
            case "L":
                holes.Add(new Tuple<double, double>(curPos.Item1, curPos.Item2 - steps));
                return holes;
            case "3":
            case "U":
                holes.Add(new Tuple<double, double>(curPos.Item1 - steps, curPos.Item2));
                return holes;
            default:
                throw new ArgumentException($"Invalid direction {direction}");
        }
    }
}
