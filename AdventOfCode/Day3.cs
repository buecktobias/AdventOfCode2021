using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    namespace Day3
    {
        public class BinaryNumberConverter
        {
            internal static int ConvertToDecimal(List<char> binary)
            {
                var decimalNumber = 0;
                var currentPower = binary.Count - 1;
                for (var i = 0; i < binary.Count; i++)
                {
                    if (binary[i] == '1') decimalNumber += (int) Math.Pow(2, currentPower);

                    currentPower--;
                }

                return decimalNumber;
            }
        }

        public class DiagnosticReport
        {
            public DiagnosticReport(List<List<char>> values)
            {
                Values = values;
            }

            internal List<List<char>> Values { get; set; }

            internal int GetXLength()
            {
                if (Values.Count >= 0)
                    return Values[0].Count;
                return 0;
            }

            internal void removeRecord(List<char> record)
            {
                Values.Remove(record);
            }

            internal void removeRecords(List<List<char>> record)
            {
                foreach (var r in record) removeRecord(r);
            }
        }

        public class PowerConsumptionCalculator
        {
            internal PowerConsumptionCalculator(DiagnosticReport diagnosticReport)
            {
                DiagnosticReport = diagnosticReport;
            }

            private DiagnosticReport DiagnosticReport { get; }

            internal int CalculateEpsilonScore()
            {
                var binaryEpsilonScore = new List<char>();
                for (var i = 0; i < DiagnosticReport.GetXLength(); i++)
                {
                    var listAtIndexI = ListDivider.GetListAtIndexFrom2DList(DiagnosticReport.Values, i);
                    var mostOftenOccuring =
                        ListCounter.GetElementMostOftenOccuring(listAtIndexI, new List<char> {'0', '1'}, '1');
                    binaryEpsilonScore.Add(mostOftenOccuring);
                }

                return BinaryNumberConverter.ConvertToDecimal(binaryEpsilonScore);
            }

            internal int CalculateGammaScore()
            {
                var binaryGammaScore = new List<char>();
                for (var i = 0; i < DiagnosticReport.GetXLength(); i++)
                {
                    var listAtIndexI = ListDivider.GetListAtIndexFrom2DList(DiagnosticReport.Values, i);
                    var mostOftenOccuring =
                        ListCounter.GetElementLeastOftenOccuring(listAtIndexI, new List<char> {'0', '1'}, '1');
                    binaryGammaScore.Add(mostOftenOccuring);
                }

                return BinaryNumberConverter.ConvertToDecimal(binaryGammaScore);
            }

            internal int CalculateScore()
            {
                return CalculateEpsilonScore() * CalculateGammaScore();
            }
        }

        public class LifeSupportRating
        {
            internal LifeSupportRating(DiagnosticReport diagnosticReport)
            {
                DiagnosticReport = diagnosticReport;
            }

            private DiagnosticReport DiagnosticReport { get; }

            internal int GetOxygenLevel()
            {
                var recordsStillPossible = DiagnosticReport.Values;
                while (recordsStillPossible.Count > 1)
                    for (var i = 0; i < DiagnosticReport.GetXLength(); i++)
                    {
                        if (recordsStillPossible.Count <= 1) break;
                        var elementMostOftenOccuring = ListCounter.GetElementMostOftenOccuring(
                            ListDivider.GetListAtIndexFrom2DList(recordsStillPossible, i),
                            new List<char> {'0', '1'},
                            '1');
                        recordsStillPossible =
                            ListFilter.KeepAllElements(recordsStillPossible, elementMostOftenOccuring, i);
                    }

                return BinaryNumberConverter.ConvertToDecimal(recordsStillPossible[0]);
            }

            internal int GetCo2Level()
            {
                var recordsStillPossible = DiagnosticReport.Values;
                var lengthOfDiagnosticReport = DiagnosticReport.GetXLength();
                while (recordsStillPossible.Count > 1)
                    for (var i = 0; i < lengthOfDiagnosticReport; i++)
                    {
                        if (recordsStillPossible.Count <= 1) break;
                        var elementLeastOftenOccuring = ListCounter.GetElementLeastOftenOccuring(
                            ListDivider.GetListAtIndexFrom2DList(recordsStillPossible, i),
                            new List<char> {'0', '1'},
                            '0');
                        recordsStillPossible =
                            ListFilter.KeepAllElements(recordsStillPossible, elementLeastOftenOccuring, i);
                    }

                return BinaryNumberConverter.ConvertToDecimal(recordsStillPossible[0]);
            }

            internal int CalculateScore()
            {
                return GetCo2Level() * GetOxygenLevel();
            }
        }

        public class ListFilter
        {
            internal static List<List<char>> RemoveAllElements(List<List<char>> records, char character, int index)
            {
                foreach (var record in records)
                    if (record[index] == character)
                        records.Remove(record);

                return records;
            }

            internal static List<List<char>> KeepAllElements(List<List<char>> records, char character, int index)
            {
                var recordsCopy = new List<List<char>>();
                for (var i = 0; i < records.Count; i++)
                    if (records[i][index] == character)
                        recordsCopy.Add(records[i]);

                return recordsCopy;
            }
        }


        public class ListCounter
        {
            internal static char GetElementLeastOftenOccuring(List<char> list, List<char> elementsToSearch,
                char sameAmount)
            {
                if (elementsToSearch.Count == 0) throw new Exception("Must be at Least Length 1");

                var currentBestElement = elementsToSearch[0];
                var currentBestElementCount = 99999999;
                foreach (var element in elementsToSearch)
                    if (CountNumberOfOccurences(list, element) < currentBestElementCount)
                    {
                        currentBestElement = element;
                        currentBestElementCount = CountNumberOfOccurences(list, element);
                    }
                    else if (CountNumberOfOccurences(list, element) == currentBestElementCount)
                    {
                        currentBestElement = sameAmount;
                    }

                return currentBestElement;
            }

            internal static char GetElementMostOftenOccuring(List<char> list, List<char> elementsToSearch,
                char sameAmounnt)
            {
                if (elementsToSearch.Count == 0) throw new Exception("Must be at Least Length 1");

                var currentBestElement = elementsToSearch[0];
                var currentBestElementCount = 0;
                foreach (var element in elementsToSearch)
                    if (CountNumberOfOccurences(list, element) > currentBestElementCount)
                    {
                        currentBestElement = element;
                        currentBestElementCount = CountNumberOfOccurences(list, element);
                    }
                    else if (CountNumberOfOccurences(list, element) == currentBestElementCount)
                    {
                        currentBestElement = sameAmounnt;
                    }

                return currentBestElement;
            }

            internal static int CountNumberOfOccurences(List<char> list, char searchedElement)
            {
                var counter = 0;
                foreach (var c in list)
                    if (c == searchedElement)
                        counter++;

                return counter;
            }
        }


        public class InputReader
        {
            internal static DiagnosticReport ReadDiagnosticReportFromFile(string filename)
            {
                var resultBinary2DList = new List<List<char>>();
                var lines = File.ReadFileLines(filename);
                foreach (var line in lines)
                {
                    var singleCharacters = line.ToCharArray().ToList();
                    resultBinary2DList.Add(singleCharacters);
                }

                return new DiagnosticReport(resultBinary2DList);
            }
        }

        public class Day3
        {
            public static void Part1()
            {
                var powerConsumptionCalculator =
                    new LifeSupportRating(InputReader.ReadDiagnosticReportFromFile("day3.txt"));
                Console.WriteLine(powerConsumptionCalculator.CalculateScore());
            }
        }
    }
}