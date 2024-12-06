﻿using System.Diagnostics;

namespace AoC24
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int day = 6;
            int part = 2;
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
    }
}