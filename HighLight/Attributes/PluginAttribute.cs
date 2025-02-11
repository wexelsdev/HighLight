namespace HighLight.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PluginAttribute : Attribute
{
    public string Name { get; }
    public string Description { get; }
    public string Author { get; }
    public string Version { get; }

    public PluginAttribute(string name, string author, string version)
    {
        Name = name;
        Description = "";
        Author = author;
        Version = version;
    }
    
    public PluginAttribute(string name, string author, string version, string description)
    {
        Name = name;
        Description = description;
        Author = author;
        Version = version;
    }
}
