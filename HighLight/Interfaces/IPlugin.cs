namespace HighLight.Interfaces;

public interface IPlugin
{
    string Name { get; }
    string Description { get; }
    string Author { get; }
    Version? Version { get; }
    
    void OnEnable();
    void OnDisable();
    void OnReloaded();
}