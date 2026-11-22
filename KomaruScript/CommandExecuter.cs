namespace KomaruScript;

public static class CommandExecuter
{
    private const string FUNCTION = "f";
    private const string LOG = "log";

    // Returns number of hunger consume
    public static int ExecuteLine(string line, string[] afterCommandContext)
    {
        line = line.Trim();

        var commands = new List<string>();
        string command = string.Empty;
        bool quote = false;
        bool parentheses = false;

        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == '"')
            {
                quote = !quote;
                if (!quote)
                {
                    commands.Add(command);
                    command = string.Empty;
                }
            }
            else if (line[i] == '(')
            {
                parentheses = true;
                
                commands.Add(command);
                command = string.Empty;
            }
            else if (line[i] == ')')
            {
                parentheses = false;
                
                commands.Add(command);
                command = string.Empty;
            }
            else if (line[i] == ' ' && !quote && !parentheses)
            {
                commands.Add(command);
                command = string.Empty;
            }
            else if (i == line.Length - 1)
            {
                if (quote || parentheses)
                {
                    ErrorLogger.Log("Unclosed quote or parentheses", ErrorLevel.Error);
                    return 0;
                }

                command += line[i];
                commands.Add(command);
                command = string.Empty;
            }
            else
            {
                command += line[i];
            }
        }

        if (commands.Count == 0)
        {
            return 0;
        }

        switch (commands[0])
        {
            case FUNCTION:
                FunctionsManager.RegisterFunction(commands[1], afterCommandContext);
                return 30;
            case LOG:
                Console.WriteLine(commands[1]);
                return 10;
            default:
                // Trying to search function
                bool isFunc = FunctionsManager.FuncToCommands.TryGetValue(commands[0], out string[]? function);
                if (isFunc)
                {
                    int funcHunger = 0;

                    foreach (var funcCommand in function ?? [])
                    {
                        funcHunger += ExecuteLine(funcCommand, []);
                    }

                    return 10 + funcHunger;
                }

                ErrorLogger.Log($"Unknown command: {commands[0]}", ErrorLevel.Error);
                break;
        }

        return 0;
    }
}