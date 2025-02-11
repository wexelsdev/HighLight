using HighLight.Attributes;
using HighLight.Interfaces;

namespace SimplePlugin.Commands;

[Command]
public class Info : ICommand
{
    public string Name => "Info";
    public string[] Aliases => [];
    public string Description => "Shows info";
    
    public bool Execute(string[] args, out string? response)
    {
        response = "Its a simple plugin!";
        return true;
    }
}