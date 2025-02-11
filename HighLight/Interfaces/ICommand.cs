namespace HighLight.Interfaces;

public interface ICommand
{
    public string Name { get; }
    public string[] Aliases { get; }
    public string Description { get; }
    
    public bool Execute(string[] args, out string? response);
}