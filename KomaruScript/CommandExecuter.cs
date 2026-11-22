namespace KomaruScript;

public static class CommandExecuter
{
    private const string FUNCTION = "f";
    private const string LOG = "log";

    // Data types
    private const string INTEGER = "int";
    private const string STRING = "str";
    private const string BOOLEAN = "bool";

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
            if (line[i] == '$' && quote)
            {
                bool closedInterpolation = false;

                string varName = string.Empty;
                int charsSkip = 0;

                for (int j = i + 1; j < line.Length; j++)
                {
                    if (line[j] == ' ' || line[j] == '"')
                    {
                        break;
                    }
                    else if (line[j] == '$')
                    {
                        closedInterpolation = true;
                        break;
                    }

                    varName += line[j];
                    charsSkip++;
                }

                if (closedInterpolation == false)
                {
                    ErrorLogger.Log("Invalid interpolation syntaxis", ErrorLevel.Error);
                    return 0;
                }

                var varPos = VariabliesManager.FindVariable(varName);
                string value = string.Empty;
                if (varPos.Length == 2)
                {
                    switch (varPos[0])
                    {
                        case 1:
                            value = VariabliesManager.Integers.Values.ElementAt(varPos[1]).ToString();
                            break;
                        case 2:
                            value = VariabliesManager.Strings.Values.ElementAt(varPos[1]);
                            break;
                        case 3:
                            value = VariabliesManager.Booleans.Values.ElementAt(varPos[1]).ToString();
                            break;
                    }
                }
                else
                {
                    ErrorLogger.Log($"Cannot find variable: {varName}", ErrorLevel.Error);
                    return 0;
                }

                command += value;
                i += charsSkip + 1;
            }
            else if (line[i] == '"')
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
            case INTEGER:
                {
                    string name = commands[1];
                    int value = 0;
                    if (commands.Count > 3 && commands[2] == "=")
                    {
                        bool canParse = int.TryParse(commands[3], out value);
                        if (!canParse)
                        {
                            ErrorLogger.Log($"Error in parsing to int: {commands[3]}", ErrorLevel.Error);
                            return 0;
                        }
                    }

                    VariabliesManager.RegisterVariable(name, value);
                }
                return 5;
            case STRING:
                {
                    string name = commands[1];
                    string value = string.Empty;
                    if (commands.Count > 3 && commands[2] == "=")
                    {
                        value = commands[3];
                    }

                    VariabliesManager.RegisterVariable(name, value);
                }
                return 10;
            case BOOLEAN:
                {
                    string name = commands[1];
                    bool value = false;
                    if (commands.Count > 3 && commands[2] == "=")
                    {
                        bool canParse = bool.TryParse(commands[3], out value);
                        if (!canParse)
                        {
                            ErrorLogger.Log($"Error in parsing to bool: {commands[3]}", ErrorLevel.Error);
                            return 0;
                        }
                    }

                    VariabliesManager.RegisterVariable(name, value);
                }
                return 5;
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