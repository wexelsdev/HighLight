using System.Reflection;
using HighLight.Attributes;
using HighLight.Interfaces;
using Timersky.Log;

namespace HighLight.Managers;

public static class CommandManager
{
    static CommandManager()
    {
        RegisterCommands(Assembly.GetExecutingAssembly());
    }

    internal static readonly Dictionary<string, ICommand> Commands = new();
    
    public static void HandleCommand(string[]? input)
    {
        if (input == null || input.Length == 0)
        {
            Log.Warning("No command entered.");
            return;
        }

        var commandName = input[0].ToLower();
        var args = input.Skip(1).ToArray();
        
        ICommand? command = Commands.Values.FirstOrDefault(cmd =>
            cmd.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase) ||
            cmd.Aliases.Any(alias => alias.Equals(commandName, StringComparison.OrdinalIgnoreCase)));
        
        if (command != null)
        {
            if (command.Execute(args, out var response))
            {
                if (string.IsNullOrEmpty(response)) return;
                
                Log.Info(response);
            }
            else
            {
                if (string.IsNullOrEmpty(response)) return;
                
                Log.Error(response);
            }
        }
        else
        {
            Log.Warning($"Command '{commandName}' not found.");
        }
    }

    internal static void RegisterCommands(Assembly assembly)
    {
        var commandTypes = assembly
            .GetTypes()
            .Where(t => t.GetCustomAttribute<CommandAttribute>() != null && typeof(ICommand).IsAssignableFrom(t));

        foreach (var type in commandTypes)
        {
            if (Activator.CreateInstance(type) is ICommand command)
            {
                Commands[command.Name.ToLower()] = command;
                Log.Debug($"Registered command: {command.Name}");
            }
        }
    }
}
