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
        for (int i = 0; i < script.Length; i++)
        {
            string line = script[i];
            if (line.Trim() == string.Empty)
            {
                continue;
            }

            var readingContext = new List<string>();

            // Checking for after command context avaible
            if (i + 1 < script.Length && script[i + 1].Trim() == "{")
            {
                // Calculating context commands count and getting them
                int depth = 1;
                int j = i + 2;

                for (; j < script.Length; j++)
                {
                    string contextCommand = script[j].Trim();

                    if (contextCommand == "{")
                    {
                        depth++;
                    }
                    else if (contextCommand == "}")
                    {
                        depth--;
                        if (depth == 0)
                        {
                            break;
                        }
                    }
                    else
                    {
                        readingContext.Add(contextCommand);
                    }
                }

                if (depth != 0)
                {
                    ErrorLogger.Log("Unclosed figure parentheses", ErrorLevel.Error);
                }

                i = j;
            }

            komaruHunger -= CommandExecuter.ExecuteLine(line, readingContext.ToArray());
        }
    }
}