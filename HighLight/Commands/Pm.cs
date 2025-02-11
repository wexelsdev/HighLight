using HighLight.Attributes;
using HighLight.Interfaces;
using HighLight.Managers;

namespace HighLight.Commands;

[Command]
public class Pm : ICommand
{
    public string Name => "PluginManager";
    
    public string[] Aliases => ["pm"];
    public string Description => "Allows you to manage your plugins.";
    
    public bool Execute(string[] args, out string? response)
    {
        if (args.Length < 1)
        {
            response = "Subcommand not found! use pm load, unload or reload.";
            return false;
        }
        
        switch (args[0])
        {
            case "load":
                PluginManager.LoadPlugins();
                
                response = "Done!";
                return true;
            case "unload":
                PluginManager.UnloadPlugins();
                
                response = "Done!";
                return true;
            case "reload":
                PluginManager.ReloadPlugins();
                
                response = "Done!";
                return true;
            default:
                response = "Subcommand not found! use pm load, unload or reload.";
                return false;
        }
    }
}