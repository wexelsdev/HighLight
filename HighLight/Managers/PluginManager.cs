using System.Reflection;
using HighLight.Attributes;
using HighLight.Interfaces;
using Timersky.Config;

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
            Program.Log.Debug("dll found: " + file);
            
            try
            {
                Program.Log.Debug("Trying to load plugin from " + file);
                
                var assembly = Assembly.LoadFrom(file);
                
                IEnumerable<Type> pluginTypes = assembly.GetTypes()
                    .Where(t => t.IsClass && t.IsSubclassOf(typeof(Plugin)) && !t.IsAbstract);

                var enumerable = pluginTypes.ToList();
                
                Program.Log.Debug("Found " + enumerable.Count + " plugins");
                
                foreach (var type in enumerable)
                {
                    Program.Log.Debug("Found plugin: " + type.Name);
                    var instance = Activator.CreateInstance(type);
                    if (instance != null)
                    {
                        _plugins.Add(instance);
                        Program.Log.Info($"Loaded plugin: {type.Name}");
                        ((dynamic)instance).OnEnable();
                        CommandManager.RegisterCommands(assembly);
                    }
                    else
                    {
                        Program.Log.Info("Failed to load plugin: " + type.Name);
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
