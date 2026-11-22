namespace KomaruScript;

public static class CommandExecuter
{
    private const string FUNCTION = "f";
    private const string LOG = "log";

    public static void ExecuteLine(string line)
    {
        var commands = new List<string>();
        string command = string.Empty;
        bool quote = false;

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
            else if (line[i] == ' ' && !quote)
            {
                commands.Add(command);
                command = string.Empty;
            }
            else if (i == line.Length - 1)
            {
                command += line[i];
                commands.Add(command);
                command = string.Empty;
            }
            else
            {
                command += line[i];
            }
        }

        switch (commands[0])
        {
            case FUNCTION:

                break;
            case LOG:
                Console.WriteLine(commands[1]);
                break;
            default:
                ErrorLogger.Log($"Unknown commad: {commands[0]}", ErrorLevel.Error);
                break;
        }
    }
}