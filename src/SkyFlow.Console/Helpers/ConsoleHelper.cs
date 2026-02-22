namespace SkyFlow.Console.Helpers;

/// <summary>
/// Provides utility methods for console input, output formatting, and display.
/// </summary>
public static class ConsoleHelper
{
    /// <summary>
    /// Displays the SkyFlow welcome banner.
    /// </summary>
    public static void DisplayBanner()
    {
        System.Console.Clear();
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine(@"

          ███████╗██╗  ██╗██╗   ██╗███████╗██╗      ██████╗ ██╗    ██╗          
          ██╔════╝██║ ██╔╝╚██╗ ██╔╝██╔════╝██║     ██╔═══██╗██║    ██║           
          ███████╗█████╔╝  ╚████╔╝ █████╗  ██║     ██║   ██║██║ █╗ ██║           
          ╚════██║██╔═██╗   ╚██╔╝  ██╔══╝  ██║     ██║   ██║██║███╗██║           
          ███████║██║  ██╗   ██║   ██║     ███████╗╚██████╔╝╚███╔███╔╝           
          ╚══════╝╚═╝  ╚═╝   ╚═╝   ╚═╝     ╚══════╝ ╚═════╝  ╚══╝╚══╝            
          
          Terminal Manager v0.2.0
");
        System.Console.ResetColor();
    }

    /// <summary>
    /// Displays the farewell message on exit.
    /// </summary>
    public static void DisplayFarewell()
    {
        System.Console.WriteLine();
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine("  Thank you for using SkyFlow. Safe travels!");
        System.Console.ResetColor();
        System.Console.WriteLine();
    }

    /// <summary>
    /// Displays a section divider with an optional title.
    /// </summary>
    /// <param name="title">The section title to display.</param>
    public static void DisplayDivider(string? title = null)
    {
        System.Console.WriteLine();
        if (title != null)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine($"  ── {title} ──");
            System.Console.ResetColor();
        }
        else
        {
            System.Console.WriteLine("  ──────────────────────────────────────────");
        }
        System.Console.WriteLine();
    }

    /// <summary>
    /// Prompts the user for input and returns the entered string.
    /// </summary>
    /// <param name="prompt">The prompt text to display.</param>
    /// <returns>The user's input string.</returns>
    public static string Prompt(string prompt)
    {
        System.Console.Write($"  {prompt}: ");
        return System.Console.ReadLine()?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Prompts the user for a password with masked input.
    /// </summary>
    /// <param name="prompt">The prompt text to display.</param>
    /// <returns>The entered password string.</returns>
    public static string PromptPassword(string prompt)
    {
        System.Console.Write($"  {prompt}: ");
        var password = string.Empty;
        ConsoleKeyInfo key;

        do
        {
            key = System.Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[..^1];
                System.Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                System.Console.Write("*");
            }
        }
        while (key.Key != ConsoleKey.Enter);

        System.Console.WriteLine();
        return password;
    }

    /// <summary>
    /// Displays a success message in green.
    /// </summary>
    /// <param name="message">The success message.</param>
    public static void Success(string message)
    {
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine($"  ✓ {message}");
        System.Console.ResetColor();
    }

    /// <summary>
    /// Displays an error message in red.
    /// </summary>
    /// <param name="message">The error message.</param>
    public static void Error(string message)
    {
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine($"  ✗ {message}");
        System.Console.ResetColor();
    }

    /// <summary>
    /// Displays a warning message in yellow.
    /// </summary>
    /// <param name="message">The warning message.</param>
    public static void Warning(string message)
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine($"  ⚠ {message}");
        System.Console.ResetColor();
    }

    /// <summary>
    /// Displays an informational message in cyan.
    /// </summary>
    /// <param name="message">The informational message.</param>
    public static void Info(string message)
    {
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine($"  ℹ {message}");
        System.Console.ResetColor();
    }

    /// <summary>
    /// Waits for the user to press Enter before continuing.
    /// </summary>
    public static void PressEnterToContinue()
    {
        System.Console.WriteLine();
        System.Console.Write("  Press Enter to continue...");
        System.Console.ReadLine();
    }

    /// <summary>
    /// Displays a numbered menu and returns the user's validated choice.
    /// </summary>
    /// <param name="title">The menu title.</param>
    /// <param name="options">The menu options.</param>
    /// <returns>The 1-based selected option number, or -1 if invalid.</returns>
    public static int DisplayMenu(string title, string[] options)
    {
        DisplayDivider(title);

        for (int i = 0; i < options.Length; i++)
        {
            System.Console.WriteLine($"  [{i + 1}] {options[i]}");
        }

        System.Console.WriteLine();
        var input = Prompt("Select an option");

        if (int.TryParse(input, out int choice) && choice >= 1 && choice <= options.Length)
            return choice;

        Error("Invalid option. Please try again.");
        return -1;
    }
}
