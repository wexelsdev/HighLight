using System.Reflection;
using HighLight.Attributes;
using HighLight.Interfaces;

namespace HighLight.Managers;

public static class PluginManager
{
    private static readonly List<object> _plugins = new();

    public static string DefaultPluginsPath { get; private set; } = $"{AppDomain.CurrentDomain.BaseDirectory}Plugins\\";

    public static void LoadPlugins(string directory = "")
    {
        if (string.IsNullOrEmpty(directory))
        {
            directory = DefaultPluginsPath;
        }
        
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var pluginFiles = Directory.GetFiles(directory, "*.dll");
        foreach (var file in pluginFiles)
        {
            try
            {
                var assembly = Assembly.LoadFrom(file);
                var pluginTypes = assembly.GetTypes()
                    .Where(t => t.GetCustomAttribute<PluginAttribute>() != null &&
                                t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(Plugin<>)));

                foreach (var type in pluginTypes)
                {
                    var instance = Activator.CreateInstance(type);
                    if (instance != null)
                    {
                        _plugins.Add(instance);
                        Program.Log.Debug($"Loaded plugin: {type.Name}");
                        ((dynamic)instance).OnEnable();
                        CommandManager.RegisterCommands(assembly);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Log.Error($"Failed to load plugin from {file}: {ex.Message}");
            }
        }
    }

    public static void UnloadPlugins()
    {
        foreach (var plugin in _plugins)
        {
            ((dynamic)plugin).OnDisable();
        }
        _plugins.Clear();
    }

    public static void ReloadPlugins()
    {
        foreach (var plugin in _plugins)
        {
            ((dynamic)plugin).OnReloaded();
        }
    }
}
