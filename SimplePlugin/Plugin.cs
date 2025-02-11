using HighLight.Attributes;

namespace SimplePlugin;

[Plugin("Simple Plugin", "Ivan Timersky", "1.0.0")]
public class Plugin : HighLight.Plugin<Config>
{
    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public override void OnReloaded()
    {
        base.OnReloaded();
    }
}