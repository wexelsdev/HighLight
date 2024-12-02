using Timersky.Config;

namespace HighLight.Interfaces;

public interface IPlugin<T> where T : IConfig
{
    public string Name { get; }
    public string Description { get; }
    public string Author { get; }
    public Version Version { get; }
    public T Config { get; }
    
    public void OnEnable();
    public void OnDisable();
}