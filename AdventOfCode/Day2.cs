using System;
using System.Collections.Generic;
using AdventOfCode.SubMarine;

namespace AdventOfCode
{
    namespace SubMarine
    {
        public sealed class SubMarine
        {
            internal int DepthPosition { get; set; }
            internal int XPosition { get; set; }

            public SubMarine()
            {
                DepthPosition = 0;
                XPosition = 0;
            }
        }

        public abstract class Command
        {
            protected int Argument { get; }

            protected Command(int argument)
            {
                Argument = argument;
            }

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

            public static List<Command> ReadCommands(string filename)
            {
                var inputLines = File.ReadFileLines(filename);
                var commands = new List<Command>();
                
                foreach (var inputLine in inputLines)
                {
                    commands.Add(ReadCommand(inputLine));
                }

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
            foreach (var command in commands)
            {
                command.Execute(subMarine);
            }

            return subMarine.DepthPosition * subMarine.XPosition;
        }
    }
}