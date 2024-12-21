using System.Diagnostics;

namespace AoC24
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int day = 21;
            int part = 1;
            bool test = false;
            int testNum = 0;

            string input = "./Input/day" + day.ToString("00");
            input += (test) ? "_test" + (testNum > 0 ? testNum.ToString() : "") + ".txt" : ".txt";

            Console.WriteLine("AoC 2024 - Day {0} , Part {1} - Test Data {2}", day, part, test);
            Stopwatch st = new();
            st.Start();
            string result = day switch
            {
                1 => day1(input, part),
                2 => day2(input, part),
                3 => day3(input, part),
                4 => day4(input, part),
                5 => day5(input, part),
                6 => day6(input, part),
                7 => day7(input, part),
                8 => day8(input, part),
                9 => day9(input, part),
                10 => day10(input, part),
                11 => day11(input, part),
                12 => day12(input, part),
                13 => day13(input, part),
                14 => day14(input, part),
                15 => day15(input, part),
                16 => day16(input, part),
                17 => day17(input, part),
                18 => day18(input, part),
                19 => day19(input, part),
                20 => day20(input, part),
                21 => day21(input, part),
                _ => throw new ArgumentException("Wrong day number - unimplemented")
            };
            st.Stop();
            Console.WriteLine("Result : {0}", result);
            Console.WriteLine("Elapsed : {0}", st.Elapsed.TotalSeconds);
        }

        static string day1(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day01.LocationSolver daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day2(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day02.ReportChecker daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day3(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day03.MemoryParser daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day4(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day04.LetterSoup daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day5(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day05.PageOrderChecker daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day6(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day06.GuardPredictor daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day7(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day07.RopeBridgeCalibrator daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day8(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day08.AntennaPlacer daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day9(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day09.Defragv2 daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day10(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day10.LavaHiker daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day11(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day11.StoneBlinker daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day12(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day12.GardenChecker daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day13(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day13.ClawHacker daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day14(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day14.BathroomPatrol daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day15(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day15.Warehouse daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day16(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day16.MazeRunner daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day17(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day17.ThreeBitComputer daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day18(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day18.PixelMaze daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day19(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day19.TowelDesigner daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day20(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day20.CheaterMaze daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day21(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day21.KeyPadPusher daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }
    }
}