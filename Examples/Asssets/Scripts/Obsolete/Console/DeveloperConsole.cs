using Utilities.DeveloperConsole.Commands;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Utilities.DeveloperConsole
{
    public class DeveloperConsole
    {
        private readonly string prefix;
        private readonly IEnumerable<IConsoleCommands> commands;

        public DeveloperConsole(string prefix, IEnumerable<IConsoleCommands> commands)
        {
            this.prefix = prefix;
            this.commands = commands;
        }

        public void ProccessCommand(string inputValue)
        {
            if (!inputValue.StartsWith(prefix))
            {
                return;
            }

            inputValue = inputValue.Remove(0, prefix.Length);

            string[] inputSplit = inputValue.Split(' ');

            string commandInput = inputSplit[0];
            string[] args = inputSplit.Skip(1).ToArray();

            ProccessCommand(commandInput, args);
        }

        public void ProccessCommand(string commandInput, string[] args)
        {
            foreach (var command in commands)
            {
                if (!commandInput.Equals(command.CommandWord, System.StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (command.Process(args))
                {
                    return;
                }
            }
        }

    }
}

