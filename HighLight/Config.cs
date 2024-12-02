using Timersky.Config;
using Tomlet.Attributes;

namespace HighLight;

public sealed class Config : IConfig
{
    [TomlProperty("debug")] public bool Debug { get; set; } = true;
}