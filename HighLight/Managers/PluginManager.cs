using System.Reflection;
using HighLight.Attributes;
using HighLight.Interfaces;
using Timersky.Config;
using Timersky.Log;

namespace HighLight.Managers;

public static class PluginManager
{
    private static readonly List<object> _plugins = new();

    public static string DefaultPluginsPath { get; private set; } = $"{AppDomain.CurrentDomain.BaseDirectory}Plugins/";

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

        Log.Info("Loading plugins");
        
        var pluginFiles = Directory.GetFiles(directory, "*.dll");
        foreach (var file in pluginFiles)
        {
            try
            {
                var assembly = Assembly.LoadFrom(file);
                
                IEnumerable<Type> pluginTypes = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract 
                                          && t.BaseType is { IsGenericType: true }
                                          && t.GetCustomAttribute<PluginAttribute>() != null);
                
                var enumerable = pluginTypes.ToList();
                
                foreach (var type in enumerable)
                {
                    var instance = Activator.CreateInstance(type);
                    if (instance != null)
                    {
                        _plugins.Add(instance);
                        ((dynamic)instance).OnEnable();
                        CommandManager.RegisterCommands(assembly);
                    }
                    else
                    {
                        Log.Info("Failed to load plugin: " + type.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to load plugin from {file}: {ex}");
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
        _plugins.Clear();

        Log.Info("Loading plugins");
        
        var pluginFiles = Directory.GetFiles(DefaultPluginsPath, "*.dll");
        foreach (var file in pluginFiles)
        {
            try
            {
                var assembly = Assembly.LoadFrom(file);
                
                IEnumerable<Type> pluginTypes = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract 
                                          && t.BaseType is { IsGenericType: true }
                                          && t.GetCustomAttribute<PluginAttribute>() != null);
                
                var enumerable = pluginTypes.ToList();
                
                foreach (var type in enumerable)
                {
                    var instance = Activator.CreateInstance(type);
                    if (instance != null)
                    {
                        _plugins.Add(instance);
                        ((dynamic)instance).OnReloaded();
                        CommandManager.RegisterCommands(assembly);
                    }
                    else
                    {
                        Log.Info("Failed to load plugin: " + type.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to load plugin from {file}: {ex}");
            }
        }
    }
}
