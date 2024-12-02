using System.Reflection;
using HighLight.Attributes;
using HighLight.Interfaces;

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
            Program.Log.Warning("No command entered.");
            return;
        }

        var commandName = input[0].ToLower();
        var args = input.Skip(1).ToArray();

        if (Commands.TryGetValue(commandName, out var command))
        {
            if (command.Execute(args, out var response))
            {
                if (string.IsNullOrEmpty(response)) return;
                
                Program.Log.Info(response);
            }
            else
            {
                if (string.IsNullOrEmpty(response)) return;
                
                Program.Log.Error(response);
            }
        }
        else
        {
            Program.Log.Warning($"Command '{commandName}' not found.");
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
                Program.Log.Debug($"Registered command: {command.Name}");
            }
        }
    }
}
