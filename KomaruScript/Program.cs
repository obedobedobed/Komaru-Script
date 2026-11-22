namespace KomaruScript;

public static class Program
{
    private static int komaruHunger = 100;

    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            ErrorLogger.Log("Not specified script path", ErrorLevel.Fatal);
            return;
        }

        string scriptPath = args[0];
        string[] script;

        if (!File.Exists(scriptPath))
        {
            ErrorLogger.Log("Invalid script path", ErrorLevel.Fatal);
            return;
        }
        else
        {
            script = File.ReadAllLines(scriptPath);
        }

        ExecuteScript(script);
    }

    private static void ExecuteScript(string[] script)
    {
        foreach (var line in script)
        {
            CommandExecuter.ExecuteLine(line);
        }
    }
}