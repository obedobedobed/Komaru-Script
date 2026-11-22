namespace KomaruScript;

public static class FunctionsManager
{
    public static Dictionary<string, string[]> FuncToCommands { get; private set; } = new Dictionary<string, string[]>();

    public static void RegisterFunction(string name, string[] commands)
    {
        FuncToCommands.Add(name, commands);
    }
}