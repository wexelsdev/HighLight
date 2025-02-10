using HighLight.Interfaces;
using HighLight.Managers;
using Timersky.Config;

namespace HighLight;

public class Plugin
{
    public virtual string Name { get; }
    public virtual string Description { get; }
    public virtual string Author { get; }
    public virtual Version Version { get; }
    //public TConfig? Config { get; private set; }
    
    public virtual void OnEnable()
    {
        //Config = ConfigManager.LoadConfig<TConfig>($"{PluginManager.DefaultPluginsPath}{Name.ToLower().Replace(' ', '_')}.toml");
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