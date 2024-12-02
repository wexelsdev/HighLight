namespace HighLight.Interfaces;

public interface ICommand
{
    public string Name { get; }
    public string Desc { get; }
    public bool Execute(string[] args, out string response);
}