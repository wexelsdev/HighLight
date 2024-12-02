using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HighLight.Attributes;
using HighLight.Interfaces;
using Timersky.Log;

namespace HighLight.Managers;

public static class PluginManager
{
    private static readonly List<object> _plugins = new();

    public static void LoadPlugins(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Program.Log.Warning($"Plugin directory '{directory}' does not exist.");
            return;
        }

        var pluginFiles = Directory.GetFiles(directory, "*.dll");
        foreach (var file in pluginFiles)
        {
            try
            {
                var assembly = Assembly.LoadFrom(file);
                var pluginTypes = assembly.GetTypes()
                    .Where(t => t.GetCustomAttribute<PluginAttribute>() != null &&
                                t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPlugin<>)));

                foreach (var type in pluginTypes)
                {
                    var instance = Activator.CreateInstance(type);
                    if (instance != null)
                    {
                        _plugins.Add(instance);
                        Program.Log.Debug($"Loaded plugin: {type.Name}");
                        ((dynamic)instance).OnEnable();
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
