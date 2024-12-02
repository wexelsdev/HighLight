using HighLight.Interfaces;
using HighLight.Managers;
using Timersky.Config;

namespace HighLight;

public class Plugin<T> : IPlugin where T : IConfig
{
    public virtual string Name { get; }
    public virtual string Description { get; }
    public virtual string Author { get; }
    public virtual Version Version { get; }
    public T Config { get; private set; }
    
    public virtual void OnEnable()
    {
        Config = ConfigManager.LoadConfig<T>($"{PluginManager.DefaultPluginsPath}{Name}.toml");
        Program.Log.Debug($"Loaded {Name}.");
    }

    public virtual void OnDisable()
    {
        Program.Log.Debug($"Unloaded {Name}.");
    }

    public virtual void OnReloaded()
    {
        Program.Log.Info($"{Name} reloaded.");
    }
}