using System.Collections.Generic;

namespace AdventOfCode
{
    public class File
    {
        public static string[] ReadFileLines(string filename)
        {
            return System.IO.File.ReadAllLines(@"C:\Users\Tobi\RiderProjects\AdventOfCode2021\input\" + filename);
        }

        public static List<int> ReadFileLinesAsIntegers(string filename)
        {
            var stringArrayLines = ReadFileLines(filename);
            var integerArrayLines = new List<int>();
            foreach (var stringLine in stringArrayLines) integerArrayLines.Add(int.Parse(stringLine));

            return integerArrayLines;
        }
    }
}