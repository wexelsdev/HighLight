using System.Reflection;
using HighLight.Attributes;
using HighLight.Interfaces;
using Timersky.Config;
using Timersky.Log;

namespace HighLight.Managers;

public static class PluginManager
{
    private static readonly List<object> Plugins = new();

    public static string DefaultPluginsPath { get; set; } = $"{AppDomain.CurrentDomain.BaseDirectory}Plugins/";

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
                var pluginTypes = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.BaseType?.IsGenericType == true
                                && t.GetCustomAttribute<PluginAttribute>() != null)
                    .ToList();

                foreach (var type in pluginTypes)
                {
                    object? instance = Activator.CreateInstance(type);
                    if (instance == null)
                    {
                        Log.Error($"Failed to instantiate plugin: {type.Name}");
                        continue;
                    }

                    var nameProperty = type.GetProperty("Name");
                    if (nameProperty == null)
                    {
                        Log.Error($"Plugin {type.Name} does not have a Name property");
                        continue;
                    }

                    string? pluginName = nameProperty.GetValue(instance) as string;
                    if (string.IsNullOrEmpty(pluginName))
                    {
                        Log.Error($"Plugin {type.Name} has an invalid Name");
                        continue;
                    }

                    MethodInfo? loadConfigMethod = typeof(ConfigManager)
                        .GetMethod("LoadConfig")?
                        .MakeGenericMethod(type.BaseType!.GetGenericArguments()[0]);

                    object? configInstance = loadConfigMethod?.Invoke(null,
                        [$"{DefaultPluginsPath}{pluginName.ToLower().Replace(' ', '_')}.toml"]);

                    if (configInstance == null)
                    {
                        Log.Error($"Failed to load config for plugin {pluginName}");
                        continue;
                    }

                    var isEnabledProperty = configInstance.GetType().GetProperty("IsEnabled");
                    if (isEnabledProperty == null || !(bool)(isEnabledProperty.GetValue(configInstance) ?? false))
                    {
                        continue;
                    }

                    Plugins.Add(instance);
                    type.GetProperty("Config")?.SetValue(instance, configInstance);
                    type.GetMethod("OnEnable")?.Invoke(instance, null);
                    CommandManager.RegisterCommands(assembly);
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
        foreach (var plugin in Plugins)
        {
            ((dynamic)plugin).OnDisable();
        }
        Plugins.Clear();
    }

    public static void ReloadPlugins()
    {
        Plugins.Clear();
        CommandManager.UnregisterCommands();

        Log.Info("Reloading plugins");
        
        var pluginFiles = Directory.GetFiles(DefaultPluginsPath, "*.dll");
        foreach (var file in pluginFiles)
        {
            try
            {
                var assembly = Assembly.LoadFrom(file);
                var pluginTypes = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.BaseType?.IsGenericType == true
                                && t.GetCustomAttribute<PluginAttribute>() != null)
                    .ToList();

                foreach (var type in pluginTypes)
                {
                    object? instance = Activator.CreateInstance(type);
                    if (instance == null)
                    {
                        Log.Error($"Failed to instantiate plugin: {type.Name}");
                        continue;
                    }

                    var nameProperty = type.GetProperty("Name");
                    if (nameProperty == null)
                    {
                        Log.Error($"Plugin {type.Name} does not have a Name property");
                        continue;
                    }

                    string? pluginName = nameProperty.GetValue(instance) as string;
                    if (string.IsNullOrEmpty(pluginName))
                    {
                        Log.Error($"Plugin {type.Name} has an invalid Name");
                        continue;
                    }

                    MethodInfo? loadConfigMethod = typeof(ConfigManager)
                        .GetMethod("LoadConfig")?
                        .MakeGenericMethod(type.BaseType!.GetGenericArguments()[0]);

                    object? configInstance = loadConfigMethod?.Invoke(null,
                        [$"{DefaultPluginsPath}{pluginName.ToLower().Replace(' ', '_')}.toml"]);

                    if (configInstance == null)
                    {
                        Log.Error($"Failed to load config for plugin {pluginName}");
                        continue;
                    }

                    var isEnabledProperty = configInstance.GetType().GetProperty("IsEnabled");
                    if (isEnabledProperty == null || !(bool)(isEnabledProperty.GetValue(configInstance) ?? false))
                    {
                        continue;
                    }

                    Plugins.Add(instance);
                    type.GetProperty("Config")?.SetValue(instance, configInstance);
                    type.GetMethod("OnReloaded")?.Invoke(instance, null);
                    CommandManager.RegisterCommands(assembly);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to load plugin from {file}: {ex}");
            }
        }
    }
}
