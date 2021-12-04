using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Day4.BingoField;

namespace AdventOfCode
{
    namespace Day4
    {
        internal class BingoGame
        {
            public BingoGame(List<BingoField.BingoField> bingoFields, List<int> bingoDrawnNumbers)
            {
                BingoFields = bingoFields;
                BingoDrawnNumbers = bingoDrawnNumbers;
                CurrentTurn = 0;
            }

            internal List<BingoField.BingoField> BingoFields { get; }

            internal List<int> BingoDrawnNumbers { get; }

            internal int CurrentTurn { get; set; }

            private bool HasAnyBoardWon()
            {
                return BingoFields.Any(field => new BingoNumberChecker(field).HasFieldWon());
            }

            internal BingoField.BingoField PlayGameUntilOnlyOneBingoFieldHasNotWon()
            {
                var bingoFieldsNotWonYet = this.BingoFields.ToList();
                while (bingoFieldsNotWonYet.Count > 1)
                {
                    bingoFieldsNotWonYet = this.BingoFields.ToList();
                    NextTurn();
                    foreach (var winnerField in GetAllWinnerFields())
                    {
                        bingoFieldsNotWonYet.Remove(winnerField);
                    }
                }

                return bingoFieldsNotWonYet[0];
            }

            internal BingoField.BingoField GetLosingBingoField()
            {
                var lastBingoField = PlayGameUntilOnlyOneBingoFieldHasNotWon();
                
                while (!(new BingoNumberChecker(lastBingoField).HasFieldWon()))
                {
                    Console.WriteLine(new BingoNumberChecker(lastBingoField).HasFieldWon());
                    NextTurn();
                }
                return lastBingoField;
            }
            
            private List<BingoField.BingoField> GetAllWinnerFields()
            {

                var winningBingoFields = BingoFields.Where(field => new BingoNumberChecker(field).HasFieldWon());
                return winningBingoFields.ToList();
            }

            private BingoField.BingoField GetWinnerField()
            {
                return this.GetAllWinnerFields().ToList()[0];
            }


            private void NextTurn()
            {
                var drawnNumber = BingoDrawnNumbers[CurrentTurn];
                foreach (var bingoField in BingoFields) bingoField.DrawNumber(drawnNumber);
                CurrentTurn++;
            }

            internal BingoField.BingoField WinningBingoField()
            {
                while (!HasAnyBoardWon()) NextTurn();

                return GetWinnerField();
            }

            public override string ToString()
            {
                var finalString = "";
                finalString += "Bingo Fields \n";
                foreach (var bingoField in BingoFields) finalString += bingoField + "\n";
                finalString += "Bingo Numbers! \n";
                foreach (var bingoDrawnNumber in BingoDrawnNumbers) finalString += bingoDrawnNumber + " ";

                return finalString;
            }
        }

        namespace BingoField
        {
            public class BingoField
            {
                internal BingoField(List<List<BingoNumber>> bingoNumbers)
                {
                    Numbers = bingoNumbers;
                }

                internal List<List<BingoNumber>> Numbers { get; }

                internal void DrawNumber(int drawnNumber)
                {
                    foreach (var row in Numbers)
                    foreach (var bingoNumber in row)
                        if (bingoNumber.Number == drawnNumber)
                            bingoNumber.Drawn = true;
                }

                public override string ToString()
                {
                    var finalString = "";
                    foreach (var bingoNumbers in Numbers)
                    {
                        foreach (var bingoNumber in bingoNumbers) finalString += bingoNumber + " ";

                        finalString += "\n";
                    }

                    return finalString;
                }
            }

            public class BingoNumberChecker
            {
                public BingoNumberChecker(BingoField bingoField)
                {
                    BingoField = bingoField;
                }

                internal BingoField BingoField { get; }

                private static bool HasListNumbersWon(List<BingoNumber> bingoNumbers)
                {
                    return bingoNumbers.All(bingoNumber => bingoNumber.Drawn);
                }

                private bool HasRowWon(int rowIndex)
                {
                    return HasListNumbersWon(BingoField.Numbers[rowIndex]);
                }

                private bool HasColumnWon(int columnIndex)
                {
                    return HasListNumbersWon(ListDivider.GetListAtIndexFrom2DList(BingoField.Numbers, columnIndex));
                }

                private bool HasRowsWon()
                {
                    for (var i = 0; i < BingoField.Numbers.Count; i++)
                        if (HasRowWon(i))
                            return true;

                    return false;
                }

                private bool HasColumnsWon()
                {
                    for (var i = 0; i < BingoField.Numbers.Count; i++)
                        if (HasColumnWon(i))
                            return true;

                    return false;
                }

                internal bool HasFieldWon()
                {
                    return HasColumnsWon() || HasRowsWon();
                }
            }

            internal class BingoNumber
            {
                internal BingoNumber(int number)
                {
                    Number = number;
                    Drawn = false;
                }

                internal int Number { get; }
                internal bool Drawn { get; set; }

                public override string ToString()
                {
                    return Number.ToString();
                }
            }

            public class BingoFieldScoreCalculator
            {
                private static int GetSumOfNotMarked(BingoField bingoField)
                {
                    var currentSum = 0;
                    foreach (var bingoFieldNumber in bingoField.Numbers)
                    foreach (var bingoNumber in bingoFieldNumber)
                        if (!bingoNumber.Drawn)
                            currentSum += bingoNumber.Number;

                    return currentSum;
                }

                private static int GetLastNumberCalled(BingoGame bingoGame)
                {
                    return bingoGame.BingoDrawnNumbers[bingoGame.CurrentTurn - 1];
                }

                internal static int CalculateScore(BingoGame bingoGame, BingoField field)
                {
                    return GetLastNumberCalled(bingoGame) * GetSumOfNotMarked(field);
                }
            }
        }

        public class InputReader
        {
            internal static BingoField.BingoField ReadBingoField(List<string> lines)
            {
                var bingoNumbers = new List<List<BingoNumber>>();
                foreach (var line in lines)
                {
                    var row = new List<BingoNumber>();
                    var mc = Regex.Matches(line, "[0-9]+");

                    foreach (Match m in mc) row.Add(new BingoNumber(int.Parse(m.Value)));
                    bingoNumbers.Add(row);
                }

                return new BingoField.BingoField(bingoNumbers);
            }

            internal static BingoGame ReadGameFromFile(string filename)
            {
                var lines = File.ReadFileLines(filename);
                var bingoNumbers = lines[0];
                var bingoNumbersSplitted = bingoNumbers.Split(",");
                var bingoNumbersIntegers = bingoNumbersSplitted.Select(number => int.Parse(number)).ToList();

                var bingoFields = new List<BingoField.BingoField>();
                var currentLine = 1;
                while (currentLine < lines.Length)
                    if (lines[currentLine].Length < 5)
                    {
                        currentLine++;
                    }
                    else
                    {
                        bingoFields.Add(ReadBingoField(lines.ToList().GetRange(currentLine, 5)));
                        currentLine += 5;
                    }

                return new BingoGame(bingoFields, bingoNumbersIntegers);
            }
        }

        public class Day4
        {
            public static void Part1()
            {
                var bingoGame = InputReader.ReadGameFromFile("day4.txt");
                var winningBoard = bingoGame.WinningBingoField();
                Console.WriteLine(BingoFieldScoreCalculator.CalculateScore(bingoGame, winningBoard));
            }

            public static void Part2()
            {
                var bingoGame = InputReader.ReadGameFromFile("day4.txt");
                var losingBingoField = bingoGame.GetLosingBingoField(); 
                Console.WriteLine(BingoFieldScoreCalculator.CalculateScore(bingoGame, losingBingoField));
            }
        }
    }
}