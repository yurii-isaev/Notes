using System.Globalization;

namespace SalesCrm.Utils.Reports;

public class Logger
{
    private StreamWriter _writer;

    public Logger(string logFilePath)
    {
        _writer = new StreamWriter(logFilePath, true);
    }

    public void Log(string message)
    {
        _writer.WriteLine($"{DateTime.Now.ToString(CultureInfo.CurrentCulture)}: {message}");
    }

    public void Flush()
    {
        _writer.Flush();
    }

    private const string LogFilePath = "/Utils/Reports/error.log";

    public static void LogError(Exception exception)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory() + LogFilePath);

        using (StreamWriter file = File.Exists(filePath) ? File.AppendText(filePath) : File.CreateText(filePath))
        {
            file.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
            file.WriteLine(exception.Message);
            file.WriteLine(exception.StackTrace);
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(exception);
        Console.ResetColor();
    }

    public static void LogError(string validationError)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[Validation Error]: " + validationError);
        Console.ResetColor();
    }
}
