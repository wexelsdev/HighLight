using Timersky.Config;
using Tomlet.Attributes;

namespace SimplePlugin;

public class Config : IConfig
{
    [TomlProperty("is_enabled")] public bool IsEnabled { get; set; } = true;
}