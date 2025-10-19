namespace TaskManagement;

static class UserInput
{
    private const string YES_ANSWER_SHORT_STRING = "y";
    private const string YES_ANSWER_FULL_STRING = "yes";
    private const string NO_ANSWER_SHORT_STRING = "n";
    private const string NO_ANSWER_FULL_STRING = "no";
    
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
            if (output == YES_ANSWER_FULL_STRING || output == YES_ANSWER_SHORT_STRING) return YesNoResponse.Yes;
            if (output == NO_ANSWER_FULL_STRING || output == NO_ANSWER_SHORT_STRING) return YesNoResponse.No;
            Console.Write(errorMessage);
        }
    }
}