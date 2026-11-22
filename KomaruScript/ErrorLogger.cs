namespace KomaruScript;

public static class ErrorLogger
{
    public static void Log(string message, ErrorLevel level)
    {
        Console.ForegroundColor = level switch
        {
            ErrorLevel.Warning => ConsoleColor.Yellow,
            ErrorLevel.Error   => ConsoleColor.Red,
            ErrorLevel.Fatal   => ConsoleColor.DarkRed,
            _                  => ConsoleColor.White
        };

        char errorChar = level switch
        {
            ErrorLevel.Warning => 'W',
            ErrorLevel.Error   => 'E',
            ErrorLevel.Fatal   => 'F',
            _                  => '?'
        };

        Console.WriteLine($"[{errorChar}] Error Message: {message}");
        Console.ResetColor();
    }
}

public enum ErrorLevel
{
    Warning,
    Error,
    Fatal
}