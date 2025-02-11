using HighLight.Attributes;
using HighLight.Interfaces;
using HighLight.Managers;
using Timersky.Log;

namespace HighLight.Commands;

[Command]
public class Help : ICommand
{
    public string Name => "Help";
    public string[] Aliases => [];
    public string Description => "Show list of available commands";
    
    public bool Execute(string[] args, out string? response)
    {
        foreach (var command in CommandManager.Commands.Values)
        {
            Log.Info($"{command.Name} | {command.Description}");
        }
        
        response = "";
        return true;
    }
}