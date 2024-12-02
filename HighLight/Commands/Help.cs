using HighLight.Attributes;
using HighLight.Interfaces;
using HighLight.Managers;

namespace HighLight.Commands;

[Command]
public class Help : ICommand
{
    public string Name => "help";
    public string Desc => "Placeholder";
    public bool Execute(string[] args, out string? response)
    {
        foreach (var command in CommandManager.Commands.Values)
        {
            Program.Log.Info($"{command.Name} | {command.Desc}");
        }
        
        response = "";
        return true;
    }
}