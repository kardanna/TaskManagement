namespace TaskManagement;

static class UserInput
{
    public static string PromtString(string promtMessage, string errorMessage = "Invalid input. Please, try again: ", string parameterName = "Not specified")
    {
        Console.Write(promtMessage);
        string output;
        while (true)
        {
            output = Console.ReadLine()?.Trim() ?? throw new ArgumentNullException(parameterName, "Console input is null!");
            if (output != string.Empty) return output;
            Console.Write(errorMessage);
        }
    }

    public static int PromtInt(string promtMessage, string errorMessage = "Invalid input. Please, try again: ", string parameterName = "Not specified")
    {
        Console.Write(promtMessage);
        int output;
        while (!int.TryParse(Console.ReadLine() ?? throw new ArgumentNullException(parameterName, "Console input is null!"), out output))
        {
            Console.Write(errorMessage);
        }
        return output;
    }

    public static YesNoResponse PromtYesNo(string promtMessage, string errorMessage = "Invalid input. Please try again (y/n): ", string parameterName = "Not specified")
    {
        Console.Write(promtMessage);
        string output;
        while (true)
        {
            output = Console.ReadLine()?.Trim().ToLower() ?? throw new ArgumentNullException(parameterName, "Console input is null!");
            if (output == "yes" || output == "y") return YesNoResponse.Yes;
            if (output == "no" || output == "n") return YesNoResponse.No;
            Console.Write(errorMessage);
        }
    }
}