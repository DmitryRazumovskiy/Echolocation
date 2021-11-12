namespace Utilities.DeveloperConsole.Commands
{
    public interface IConsoleCommands 
    {
        string CommandWord
        {
            get;
        }

        bool Process(string[] args);
    }
}

