using Timersky.Config;
using Tomlet.Attributes;

namespace HighLight.Interfaces;

public interface IPluginConfig : IConfig
{
    [TomlProperty("is_enabled")] public bool IsEnabled { get; set; }
}