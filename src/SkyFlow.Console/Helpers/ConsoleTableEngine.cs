namespace SkyFlow.Console.Helpers;

/// <summary>
/// Renders tabular data with ASCII borders for clean console output.
/// Uses +, -, and | characters to create aligned, bordered tables.
/// </summary>
public static class ConsoleTableEngine
{
    /// <summary>
    /// Renders a table with the specified headers and rows.
    /// </summary>
    /// <param name="headers">The column headers.</param>
    /// <param name="rows">The data rows as string arrays.</param>
    public static void Render(string[] headers, List<string[]> rows)
    {
        if (headers.Length == 0) return;

        // Calculate column widths
        var columnWidths = new int[headers.Length];
        for (int i = 0; i < headers.Length; i++)
        {
            columnWidths[i] = headers[i].Length;
        }

        foreach (var row in rows)
        {
            for (int i = 0; i < Math.Min(row.Length, headers.Length); i++)
            {
                var cellLength = (row[i] ?? string.Empty).Length;
                if (cellLength > columnWidths[i])
                    columnWidths[i] = cellLength;
            }
        }

        // Build separator line
        var separator = "+" + string.Join("+",
            columnWidths.Select(w => new string('-', w + 2))) + "+";

        // Build header line
        var headerLine = "|" + string.Join("|",
            headers.Select((h, i) => $" {h.PadRight(columnWidths[i])} ")) + "|";

        // Print table
        System.Console.WriteLine(separator);
        System.Console.WriteLine(headerLine);
        System.Console.WriteLine(separator);

        foreach (var row in rows)
        {
            var cells = new string[headers.Length];
            for (int i = 0; i < headers.Length; i++)
            {
                cells[i] = i < row.Length ? (row[i] ?? string.Empty) : string.Empty;
            }

            var rowLine = "|" + string.Join("|",
                cells.Select((c, i) => $" {c.PadRight(columnWidths[i])} ")) + "|";
            System.Console.WriteLine(rowLine);
        }

        System.Console.WriteLine(separator);
    }
}
