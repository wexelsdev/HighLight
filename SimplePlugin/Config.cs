using HighLight.Interfaces;
using Tomlet.Attributes;

namespace SimplePlugin;

public class Config : IPluginConfig
{
    [TomlProperty("is_enabled")] public bool IsEnabled { get; set; } = true;
}