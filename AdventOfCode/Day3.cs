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
                var currentPower = binary.Count -1;
                for (var i = 0; i < binary.Count; i++)
                {
                    if (binary[i] == '1')
                    {
                        decimalNumber += (int)Math.Pow(2, currentPower);
                    }

                    currentPower--;
                }

                return decimalNumber;
            }
        }
        
        public class DiagnosticReport
        {
            internal List<List<char>> Values { get; set; }

            public DiagnosticReport(List<List<char>> values)
            {
                Values = values;
            }

            public int GetXLength()
            {
                if (Values.Count >= 0)
                {
                    return Values[0].Count;
                }
                else
                {
                    return 0;
                }
            }

        }

        public class PowerConsumptionCalculator
        {
            private DiagnosticReport DiagnosticReport { get; }
            internal PowerConsumptionCalculator(DiagnosticReport diagnosticReport)
            {
                this.DiagnosticReport = diagnosticReport;
            }

            internal int CalculateEpsilonScore()
            {
                var binaryEpsilonScore = new List<char>();
                for (var i = 0; i < DiagnosticReport.GetXLength(); i++)
                {
                    var listAtIndexI = ListDivider.GetListAtIndexFrom2DList(DiagnosticReport.Values, i);
                    var mostOftenOccuring = ListCounter.GetElementMostOftenOccuring(listAtIndexI, new List<char>() {'0', '1'});
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
                    var mostOftenOccuring = ListCounter.GetElementLeastOftenOccuring(listAtIndexI, new List<char>() {'0', '1'});
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
            internal DiagnosticReport
            internal LifeSupportRating(DiagnosticReport diagnosticReport)
            {
            }
            

        }


        public class ListCounter
        {
            
            internal static char GetElementLeastOftenOccuring(List<char> list, List<char> elementsToSearch)
            {
                if (elementsToSearch.Count == 0)
                {
                    throw new Exception("Must be at Least Length 1");
                }

                var currentBestElement = elementsToSearch[0];
                var currentBestElementCount = 99999999;
                foreach (var element in elementsToSearch)
                {
                    if (CountNumberOfOccurences(list, element) < currentBestElementCount)
                    {
                        currentBestElement = element;
                        currentBestElementCount = CountNumberOfOccurences(list, element);
                    }
                }
                return currentBestElement;
            }
            
            internal static char GetElementMostOftenOccuring(List<char> list, List<char> elementsToSearch)
            {
                if (elementsToSearch.Count == 0)
                {
                    throw new Exception("Must be at Least Length 1");
                }

                var currentBestElement = elementsToSearch[0];
                var currentBestElementCount = 0;
                foreach (var element in elementsToSearch)
                {
                    if (CountNumberOfOccurences(list, element) > currentBestElementCount)
                    {
                        currentBestElement = element;
                        currentBestElementCount = CountNumberOfOccurences(list, element);
                    }
                }
                return currentBestElement;
            }

            internal static int CountNumberOfOccurences(List<char> list, char searchedElement)
            {
                int counter = 0;
                foreach (var c in list)
                {
                    if (c == searchedElement)
                    {
                        counter++;
                    }
                }

                return counter;
            }
        }

        public class ListDivider
        {
            internal static List<char> GetListAtIndexFrom2DList(List<List<char>> twoDimensionalCharacterList, int index)
            {
                var newListDividedAtIndex = new List<char>();
                foreach (var chars in twoDimensionalCharacterList)
                {
                    newListDividedAtIndex.Add(chars[index]);
                }

                return newListDividedAtIndex;
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
                var powerConsumptionCalculator = new PowerConsumptionCalculator(InputReader.ReadDiagnosticReportFromFile("day3.txt"));
                Console.WriteLine(powerConsumptionCalculator.CalculateScore());
            }

        }
    }
}