using System;
using System.Collections.Generic;
using AdventOfCode.Day2.SubMarine;

namespace AdventOfCode
{
    namespace Day2
    {
        namespace SubMarine
        {
            public class SubMarine
            {
                public SubMarine()
                {
                    DepthPosition = 0;
                    XPosition = 0;
                }

                internal int DepthPosition { get; set; }
                internal int XPosition { get; set; }
            }

            public sealed class SubMarinePart2 : SubMarine
            {
                public SubMarinePart2()
                {
                    DepthPosition = 0;
                    XPosition = 0;
                    Aim = 0;
                }

                internal int Aim { get; set; }
            }

            public abstract class Command
            {
                protected Command(int argument)
                {
                    Argument = argument;
                }

                protected int Argument { get; }

                public abstract void Execute(SubMarine subMarine);
            }

            public sealed class Forward : Command
            {
                public Forward(int argument) : base(argument)
                {
                }

                public override void Execute(SubMarine subMarine)
                {
                    subMarine.XPosition += Argument;
                }
            }

            public sealed class ForwardPart2 : Command
            {
                public ForwardPart2(int argument) : base(argument)
                {
                }

                public override void Execute(SubMarine subMarine)
                {
                    var sub = (SubMarinePart2) subMarine;
                    subMarine.XPosition += Argument;
                    subMarine.DepthPosition += sub.Aim * Argument;
                }
            }

            public sealed class Up : Command
            {
                public Up(int argument) : base(argument)
                {
                }

                public override void Execute(SubMarine subMarine)
                {
                    subMarine.DepthPosition -= Argument;
                }
            }

            public sealed class UpPart2 : Command
            {
                public UpPart2(int argument) : base(argument)
                {
                }

                public override void Execute(SubMarine subMarine)
                {
                    var sub = (SubMarinePart2) subMarine;
                    sub.Aim -= Argument;
                }
            }

            public sealed class Down : Command
            {
                public Down(int argument) : base(argument)
                {
                }

                public override void Execute(SubMarine subMarine)
                {
                    subMarine.DepthPosition += Argument;
                }
            }

            public sealed class DownPart2 : Command
            {
                public DownPart2(int argument) : base(argument)
                {
                }

                public override void Execute(SubMarine subMarine)
                {
                    var sub = (SubMarinePart2) subMarine;
                    sub.Aim += Argument;
                }
            }


            public sealed class InputReader
            {
                private static Command ReadCommand(string stringCommand)
                {
                    var splitString = stringCommand.Split(" ");
                    var argument = int.Parse(splitString[1]);

                    return splitString[0] switch
                    {
                        "up" => new Up(argument),
                        "down" => new Down(argument),
                        "forward" => new Forward(argument),
                        _ => throw new Exception("Wrong Command!")
                    };
                }

                private static Command ReadCommandPart2(string stringCommand)
                {
                    var splitString = stringCommand.Split(" ");
                    var argument = int.Parse(splitString[1]);

                    return splitString[0] switch
                    {
                        "up" => new UpPart2(argument),
                        "down" => new DownPart2(argument),
                        "forward" => new ForwardPart2(argument),
                        _ => throw new Exception("Wrong Command!")
                    };
                }

                public static List<Command> ReadCommands(string filename)
                {
                    var inputLines = File.ReadFileLines(filename);
                    var commands = new List<Command>();

                    foreach (var inputLine in inputLines) commands.Add(ReadCommand(inputLine));

                    return commands;
                }

                public static List<Command> ReadCommandsPart2(string filename)
                {
                    var inputLines = File.ReadFileLines(filename);
                    var commands = new List<Command>();

                    foreach (var inputLine in inputLines) commands.Add(ReadCommandPart2(inputLine));

                    return commands;
                }
            }
        }

        public class Day2
        {
            public static int Part1()
            {
                var subMarine = new SubMarine.SubMarine();
                var commands = InputReader.ReadCommands("day2.txt");
                foreach (var command in commands) command.Execute(subMarine);

                return subMarine.DepthPosition * subMarine.XPosition;
            }

            public static int Part2()
            {
                var subMarine = new SubMarinePart2();
                var commands = InputReader.ReadCommandsPart2("day2.txt");
                foreach (var command in commands) command.Execute(subMarine);

                return subMarine.DepthPosition * subMarine.XPosition;
            }
        }
    }
}