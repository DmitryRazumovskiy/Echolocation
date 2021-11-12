using UnityEngine;
using Utilities.DeveloperConsole.Commands;

public abstract class ConsoleComnand : ScriptableObject, IConsoleCommands
{
    [SerializeField] private string commandWord = string.Empty;
    public string CommandWord => commandWord;

    public abstract bool Process(string[] args);
   
}
