namespace KomaruScript;

public static class VariabliesManager
{
    public static Dictionary<string, int> Integers { get; private set; } = new Dictionary<string, int>();
    public static Dictionary<string, string> Strings { get; private set; } = new Dictionary<string, string>();
    public static Dictionary<string, bool> Booleans { get; private set; } = new Dictionary<string, bool>();

    public static void RegisterVariable(string name, int value)
    {
        Integers.Add(name, value);
    }

    public static void RegisterVariable(string name, string value)
    {
        Strings.Add(name, value);
    }

    public static void RegisterVariable(string name, bool value)
    {
        Booleans.Add(name, value);
    }

    // Returns dictionary number and index
    public static int[] FindVariable(string name)
    {
        // Integers
        for (int i = 0; i < Integers.Count; i++)
        {
            if (Integers.Keys.ElementAt(i) == name)
            {
                return [1, i];
            }
        }

        // Strings
        for (int i = 0; i < Strings.Count; i++)
        {
            if (Strings.Keys.ElementAt(i) == name)
            {
                return [2, i];
            }
        }

        // Booleans
        for (int i = 0; i < Booleans.Count; i++)
        {
            if (Booleans.Keys.ElementAt(i) == name)
            {
                return [2, i];
            }
        }

        return [];
    }
}