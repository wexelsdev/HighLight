using System.Reflection;
using HighLight.Attributes;
using HighLight.Interfaces;
using HighLight.Managers;
using Timersky.Config;
using Timersky.Log;

namespace HighLight;

public class Plugin<TConfig> where TConfig : IPluginConfig
{
    public string Name { get; }
    public string Description { get; }
    public string Author { get; }
    public string Version { get; }

    protected Plugin()
    {
        var attr = GetType().GetCustomAttribute<PluginAttribute>();

        if (attr != null)
        {
            Name = attr.Name;
            Description = attr.Description;
            Author = attr.Author;
            Version = attr.Version;
        }
        else
        {
            throw new InvalidOperationException($"Plugin {GetType().Name} dont have [Plugin] attribute");
        }
    }
    
    public TConfig? Config { get; internal set; }
    
    public virtual void OnEnable()
    {
        Log.Info($"Loaded {Name}, v.{Version} by {Author}");
    }

    public virtual void OnDisable()
    {
        Log.Info($"Unloaded {Name}, v.{Version} by {Author}");
    }

    public virtual void OnReloaded()
    {
        Log.Info($"Reloaded {Name}, v.{Version} by {Author}");
    }
}