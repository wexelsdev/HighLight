using Timersky.Config;
using Timersky.Log;

namespace HighLight;

public static class Program
{
    public static Logger Log { get; set; } = new();
    public static Config? Config { get; private set; }
    public static bool IsActive { get; private set; } = true;
    

    public static void Main(string[] args)
    {
        try
        {
            Start();

            while (IsActive)
            {
                CommandHandler.HandleCommand(Log.Read()?.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            Exit(1);
        }
    }

    private static void OnProcessExit(object sender, EventArgs e)
    {
        Exit(1);
    }
    
    private static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
        e.Cancel = true;
        Exit(1);
    }
    
    private static void Start()
    {
        Log.Info("Starting...");
        
        Config = ConfigManager.LoadConfig<Config>();
        Log.DebugIsAllowed = Config.Debug;
        
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        Console.CancelKeyPress += OnCancelKeyPress;
        
        Log.Info("Ready");
    }

    public static void Exit(int exitCode)
    {
        Log.Info("Exiting...");
        
        AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
        Console.CancelKeyPress -= OnCancelKeyPress;
        
        IsActive = false;
        
        Environment.Exit(exitCode);
    }
}