using System.Diagnostics;

namespace AoC24
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int day = 11;
            int part = 1;
            bool test = !false;
            int testNum = 0;

            string input = "./Input/day" + day.ToString("00");
            input += (test) ? "_test" + (testNum > 0 ? testNum.ToString() : "") + ".txt" : ".txt";

            Console.WriteLine("AoC 2024 - Day {0} , Part {1} - Test Data {2}", day, part, test);
            Stopwatch st = new();
            st.Start();
            string result = day switch
            {
                _ => throw new ArgumentException("Wrong day number - unimplemented")
            };
            st.Stop();
            Console.WriteLine("Result : {0}", result);
            Console.WriteLine("Elapsed : {0}", st.Elapsed.TotalSeconds);
        }
    }
}