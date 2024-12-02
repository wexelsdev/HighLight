using Timersky.Config;

namespace HighLight.Interfaces;

public interface IPlugin<T> where T : IConfig
{
    string Name { get; }
    string Description { get; }
    string Author { get; }
    Version Version { get; }
    T Config { get; }
    
    void OnEnable();
    void OnDisable();
    void OnReloaded();
}